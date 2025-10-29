using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList1;

public class ViewCommand : BaseCommand
{
	public bool ShowIndex { get; set; }
	public bool ShowStatus { get; set; }
	public bool ShowDate { get; set; }
	public bool ShowAll { get; set; }

	public override void Execute()
	{
		if (ShowAll)
		{
			todoList.View(true, true, true);
		}
		else
		{
			todoList.View(ShowIndex, ShowStatus, ShowDate);
		}
	}
}
