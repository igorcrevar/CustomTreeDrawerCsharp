using CustomTreeDrawerWPF.Drawers.Interfaces;

namespace CustomTreeDrawerWPFExample
{
	class SomeCustomClass : ICustomTreeSimpleNodeInfo
	{
        private string title;

        public SomeCustomClass(string title)
		{
			this.title = title;
		}

        public string Title
        {
            get
            {
                return title;
            }
        }
    }
}
