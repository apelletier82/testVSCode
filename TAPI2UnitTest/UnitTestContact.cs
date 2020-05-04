using Xunit;
using TAPI2.DB;
using TAPI2.Entities;
using TAPI2.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using TAPI2.Services.Abstract;

namespace TestWebApi2
{
    public class UnitTestContact
    {                 
        const string contactName = "Pelletier Alexandre"; 

        private TAPIDataContext createNewDataContextInstance()
        {
            var optionsbuilder = new DbContextOptionsBuilder<TAPIDataContext>(); 
            optionsbuilder.UseSqlite("Data Source=/home/alex/Devs/SQLLiteDB/Test_TAPI.db");
            return new TAPIDataContext(optionsbuilder.Options, new TestUserIdentity());            
        }

        [Fact]
        public void CreateContact()
        {          
            using (ILoggerFactory logFact = new LoggerFactory())
            {           
                using (TAPIDataContext db = createNewDataContextInstance())
                {
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    IContactService csrv = new ContactDBService(db, logFact);
                    Contact ct = csrv.ListByName(contactName, true).FirstOrDefault();
                    
                    if (ct == null)
                    {
                        ct = new Contact(contactName);                
                        csrv.Add(ct);
                    }
                }
            }
        }


        [Fact]
        public void AddAddress()
        {   
            using (ILoggerFactory logFact = new LoggerFactory())
            {           
                using (var db = createNewDataContextInstance())
                {
                    IContactService crsrv = new ContactDBService(db, logFact);
                    Contact ct = crsrv.ListByName(contactName, true).FirstOrDefault();                
                    ct.AddAddress("Home","52D rue des Crets", "74200 Allinges");
                    crsrv.Update(ct);            
                }            
            }
        }        

        [Fact]
        public void GetSingleContact()
        {
            using (ILoggerFactory logFact = new LoggerFactory())
            {      
                using (var db = createNewDataContextInstance())
                {
                    IContactService crsrv = new ContactDBService(db, logFact);
                    Contact ct = crsrv.GetByID(1);
                    ct.UpdateName2("Test Name 2");
                    crsrv.Update(ct);
                }
            }
        }
    }
}
