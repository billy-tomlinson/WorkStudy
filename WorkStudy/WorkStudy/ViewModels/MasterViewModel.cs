using System.Windows.Input;
using WorkStudy.Pages;
using Xamarin.Forms;

namespace WorkStudy
{
	public class MasterViewModel
	{
		public ICommand NavigationCommand
		{
			get
			{
				return new Command((value) =>
				{
					// COMMENT: This is just quick demo code. Please don't put this in a production app.
					var mdp = (Application.Current.MainPage as MasterDetailPage);
					var navPage = mdp.Detail as NavigationPage;

					// Hide the Master page
					mdp.IsPresented = false;

					switch (value)
					{
						case "1":
							navPage.PushAsync(new AddOperators());
							break;
						case "2":
							navPage.PushAsync(new EditActivities());
							break;
					}
					
				});
			}
		}
	}
}
