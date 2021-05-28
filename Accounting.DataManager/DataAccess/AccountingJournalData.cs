﻿using Accounting.DataManager.Models;
using System.Collections.Generic;

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
            }
            catch (System.Exception)
            {
                _sql.RollBackTransaction();
                throw;
            }
        }
    }
}
