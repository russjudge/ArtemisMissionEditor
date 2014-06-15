
using System.Windows.Forms;
namespace EngineeringTemplateMaster
{
	public class RefreshingListBox : ListBox
	{
		public new void RefreshItem(int index)
		{
			base.RefreshItem(index);
		}

		public new void RefreshItems()
		{
			base.RefreshItems();
		}
	}
}
