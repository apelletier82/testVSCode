using Xunit;
using TAPI2.DB;
using TAPI2.Entities;
using TAPI2.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using TAPI2.Services.Abstract;

namespace TestWebApi2
{
    public class unitTestContactAsync
    {
        const string contactName = "Pelletier Alexandre Async"; 

        private TAPIDataContext createNewDataContextInstance()
        {
            var optionsbuilder = new DbContextOptionsBuilder<TAPIDataContext>(); 
            optionsbuilder.UseSqlite("Data Source=/home/alex/Devs/SQLLiteDB/Test_TAPI.db");
            return new TAPIDataContext(optionsbuilder.Options, new TestUserIdentity());            
        }

        [Fact]
        public async void CreateContactAsync()
        {      
            using (ILoggerFactory logFact = new LoggerFactory())
            {                    
                using (TAPIDataContext db = createNewDataContextInstance())
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    IContactService csrv = new ContactDBService(db, logFact);                                
                    var contactList = await csrv.ListByNameAsync(contactName, true);
                    var ct = contactList.FirstOrDefault();
                    
                    if (ct == null)
                    {
                        ct = new Contact(contactName);                
                        await csrv.AddAsync(ct);
                    }
                }
            }
        }
        [Fact]
        public async void GetSingleContact()
        {
            using (ILoggerFactory logFact = new LoggerFactory())
            {              
                using (var db = createNewDataContextInstance())
                {
                    IContactService crsrv = new ContactDBService(db, logFact);
                    Contact ct = await crsrv.GetByIDAsync(1);
                    ct.UpdateName2("Test Name 2");
                    await crsrv.UpdateAsync(ct);
                }
            }
        }

        [Fact]
        public async void AddAddress()
        {    
            using (ILoggerFactory logFact = new LoggerFactory())
            {                      
                using (var db = createNewDataContextInstance())
                {
                    IContactService crsrv = new ContactDBService(db, logFact);
                    var contactList = await crsrv.ListByNameAsync(contactName, true);
                    var ct = contactList.FirstOrDefault();
                    ct.AddAddress("Home","52D rue des Crets", "74200 Allinges");
                    await crsrv.UpdateAsync(ct);
                } 
            }           
        }        
    }
}