﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBAStats.Services
{
    public class CreateDatabase
    {
        DataBaseService DataBase;
        public DataBaseService SQliteDB
        {
            get
            {
                if (DataBase == null)
                {
                    DataBase = new DataBaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stats.db3"));
                }
                return DataBase;
            }
        }
    }
}