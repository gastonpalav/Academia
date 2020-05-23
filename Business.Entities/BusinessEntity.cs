﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class BusinessEntity
    {
        public BusinessEntity()
        {
            this.State = States.New;
                
        }

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private States _States;

        public States State
        {
            get { return _States; }
            set { _States = value; }
        }

        public enum States
        {
            Deleted,
            New,
            Modifed,
            Unmodifed,

        }




    }
}
