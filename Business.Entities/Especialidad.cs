﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
   public class Especialidad : BusinessEntity
    {
		private String _Descripcion;

		public String Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}

	}
}
