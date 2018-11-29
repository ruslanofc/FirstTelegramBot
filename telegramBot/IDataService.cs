using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telegramBot;

namespace telegramBot
{
    interface IDataService
    {
        void Save(Student entity);
        void Delete(int id);
        void Update(int id, Student entity);
        void GetAll();
    }
}