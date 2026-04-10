using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList;
public class SystemClock : IClock
{
	public DateTime Now => DateTime.Now;
}