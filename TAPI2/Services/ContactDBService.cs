using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TAPI2.DB;
using TAPI2.Entities;
using TAPI2.Exceptions;
using TAPI2.Services.Abstract;

namespace TAPI2.Services
{
    public class ContactDBService : IContactService
    {
        TAPIDataContext _dbContext;
        ILogger<ContactDBService> _logger;

        public ContactDBService(TAPIDataContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<ContactDBService>();
        }

        public Contact GetByID(long id)
            => _dbContext.Contacts
                .Where(c => c.ID == id)
                .Include(i => i.Addresses)
                .FirstOrDefault();

        public async Task<Contact> GetByIDAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
            => await _dbContext.Contacts
                    .Where(c => c.ID == id)
                    .Include(i => i.Addresses)
                    .FirstOrDefaultAsync(cancellationToken);

        public IEnumerable<Contact> List() => _dbContext.Contacts.AsEnumerable();

        public Contact FindVersion(long id, byte[] rowVersion)
            => _dbContext.Contacts
                .Where(c => (c.ID == id && c.RowVersion == rowVersion))
                .FirstOrDefault();

        public async Task<Contact> FindVersionAsync(long id, byte[] rowVersion, CancellationToken cancellationToken = default(CancellationToken))
            => await _dbContext.Contacts
                    .Where(c => (c.ID == id && c.RowVersion == rowVersion))
                    .FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<Contact>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Task<IEnumerable<Contact>> result = new Task<IEnumerable<Contact>>(() => List(), cancellationToken);
            result.Start();
            return await result;
        }

        public IEnumerable<Contact> ListByName(string name, bool strictEquality)
        {
            if (strictEquality)
                return _dbContext.Contacts
                        .Where(c => c.Name.ToLower() == name.ToLower())
                        .Include(i => i.Addresses)
                        .AsEnumerable();
            else
                return _dbContext.Contacts
                        .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                        .Include(i => i.Addresses)
                        .AsEnumerable();
        }

        public async Task<IEnumerable<Contact>> ListByNameAsync(string name, bool strictEquality, CancellationToken cancellationToken = default(CancellationToken))
        {
            Task<IEnumerable<Contact>> result = new Task<IEnumerable<Contact>>(() => ListByName(name, strictEquality), cancellationToken);
            result.Start();
            return await result;
        }

        public Contact Add(Contact instance)
        {
            try
            {
                var ct = _dbContext.Contacts.Add(instance);
                _dbContext.SaveChanges();
                return ct.Entity;
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact not created: an error occured", e);
            }
        }

        public async Task<Contact> AddAsync(Contact instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var ct = await _dbContext.Contacts.AddAsync(instance, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return ct.Entity;
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact not created: an error occured", e);
            }
        }

        public Contact Update(Contact instance)
        {
            try
            {
                if (_dbContext.Contacts.Find(instance.ID) != null)
                {
                    if (FindVersion(instance.ID, instance.RowVersion) == null)
                        throw new RowVersionException(typeof(Contact));
                }
                else
                    throw new EntityNotFoundException("Contact not found");

                var contact = _dbContext.Contacts.Update(instance);
                _dbContext.SaveChanges();

                return contact.Entity;
            }
            catch (BusinessException be)
            {
                throw be;
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact not updated: an error occured", e);
            }
        }

        public async Task<Contact> UpdateAsync(Contact instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                object[] keyValues = { instance.ID };
                if (await _dbContext.Contacts.FindAsync(keyValues, cancellationToken) != null)
                {
                    if (await FindVersionAsync(instance.ID, instance.RowVersion, cancellationToken) == null)
                        throw new RowVersionException(typeof(Contact));
                }
                else
                    throw new EntityNotFoundException("Contact not found");

                var ct = _dbContext.Contacts.Update(instance);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return ct.Entity;
            }
            catch (BusinessException be)
            {
                throw be;
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact not updated: an error occured", e);
            }
        }

        public bool Delete(Contact instance)
        {
            try
            {
                _dbContext.Contacts.Remove(instance);
                return (_dbContext.SaveChanges() != 0);
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact was not delete: an error ocured", e);
            }
        }

        public bool Delete(long id) => Delete(_dbContext.Contacts.Find(id));

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                object[] keyValues = { id };
                Contact contact = await _dbContext.Contacts.FindAsync(keyValues, cancellationToken);
                if (contact == null)
                    throw new EntityNotFoundException(string.Format("Contact ID {O} not found", id));
                    
                return await DeleteAsync(contact);
            }
            catch (ContactException se)
            {
                throw se;
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact not deleted: an error occured", e);
            }
        }
        public async Task<bool> DeleteAsync(Contact instance, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                _dbContext.Contacts.Remove(instance);
                return (await _dbContext.SaveChangesAsync(cancellationToken) != 0);
            }
            catch (Exception e)
            {
                throw new ServiceException("Contact not deleted: an error occured", e);
            }
        }
    }
}