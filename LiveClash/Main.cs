using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace TeamCAD_LiveClash

{
    class App : IExternalApplication
    {

        // class instance
        public static App thisApp = null;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        static string _path = typeof(App).Assembly.Location;

        /// <summary>
        /// Singleton external application class instance.
        /// </summary>
        internal static App _app = null;

        /// <summary>
        /// Provide access to singleton class instance.
        /// </summary>
        public static App Instance
        {
            get { return _app; }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public Result OnStartup(UIControlledApplication a)
        {


            String tabName = "TeamCAD";
            string assembly = @"C:\Users\TeamCAD-Igor\source\repos\drDzX\LiveClash\LiveClash\bin\Release\LiveClash.dll";
            a.CreateRibbonTab(tabName);

            RibbonPanel panel = a.CreateRibbonPanel(tabName, "MEP Clash Detect");

            //Dodavanje dugmica            

            PushButton pushButtonSD = panel.AddItem(new PushButtonData("Enable collision tracking", "Collision", assembly, "TeamCAD_LiveClash.RegisterUpdater")) as PushButton;




            //Slicice za komande
            //pushButtonSD.LargeImage = new BitmapImage(new Uri(@"D:\Users\TeamCAD-Igor\Pictures\KILL.png"));
            ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url,
                "http://www.teamcad.rs");
            pushButtonSD.SetContextualHelp(contextHelp);

            ////////////////////////////////////////////////////


            //ParameterUpdater _updater = new ParameterUpdater(a.ActiveAddInId.GetGUID());
            //UpdaterRegistry.RegisterUpdater(_updater, true);




            //ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);
            //ElementId paramId = new ElementId(BuiltInParameter.ROOM_AREA);

            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), filter, Element.GetChangeTypeParameter(paramId));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {

            //ParameterUpdater updater = new ParameterUpdater(a.ActiveAddInId.GetGUID());
            //UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());

            return Result.Succeeded;
        }

    }


}