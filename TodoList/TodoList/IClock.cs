using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp;

// IClock.cs
public interface IClock
{
	DateTime Now { get; }
}