using Accounting.DataManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Accounting.DataManager.DataAccess
{
    public class AccountingJournalData : IAccountingJournalData
    {
        private readonly ISqlDataAccess _sql;

        public AccountingJournalData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<AccountingJournalModel> GetByBookNumber(int bookNumber)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AccountingJournalModel, dynamic>("dbo.spAccountingJournal_GetByBookNumber", new { BrojTemeljnice = bookNumber });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<AccountingJournalModel> GetHeaders()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AccountingJournalModel, dynamic>("dbo.spAccountingJournal_GetHeaders", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<AccountingJournalModel> GetUnprocessedHeaders()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AccountingJournalModel, dynamic>("dbo.spAccountingJournal_GetUnprocessedHeaders", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<AccountingJournalModel> GetProcessedHeaders()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AccountingJournalModel, dynamic>("dbo.spAccountingJournal_GetProcessedHeaders", new { });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public List<AccountingJournalModel> GetJournalDetails(AccountingJournalModel model)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                return _sql.LoadDataInTransaction<AccountingJournalModel, dynamic>("dbo.spAccountingJournal_GetJournalDetail", 
                    new 
                    { 
                        VrstaTemeljnice = model.VrstaTemeljnice,
                        BrojTemeljnice = model.BrojTemeljnice
                    });
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Insert(List<AccountingJournalModel> journal)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spAccountingJournal_Insert", journal);

                _sql.CommitTransaction();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void Update(List<AccountingJournalModel> journal)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spAccountingJournal_Update", journal);
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public void DeleteJournal(AccountingJournalModel model)
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                _sql.SaveDataInTransaction("dbo.spAccountingJournal_Delete", new
                {
                    VrstaTemeljnice = model.VrstaTemeljnice,
                    BrojTemeljnice = model.BrojTemeljnice
                });
                _sql.CommitTransaction();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }

        public int LatestNumber()
        {
            try
            {
                _sql.StartTransaction("AccountingConnStr");

                var result = _sql.LoadDataInTransaction<int, dynamic>("dbo.spAccountingJournal_GetLatestNumber", new { });
                return result.FirstOrDefault();
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
