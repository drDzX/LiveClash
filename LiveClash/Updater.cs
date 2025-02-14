﻿using System;
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
    [Transaction(TransactionMode.Manual)]

    [Regeneration(RegenerationOption.Manual)]

    public class RegisterUpdater : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)

        {
            UIApplication a = commandData.Application;
            Document doc = a.ActiveUIDocument.Document;

            ClashUpdater updater = new ClashUpdater(a.ActiveAddInId.GetGUID());
            // UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());
            if (UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId()))
            {
                UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());
            }
            else
            {
                UpdaterRegistry.RegisterUpdater(updater, false);
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
                ElementId paramId = new ElementId(BuiltInParameter.ROOM_AREA);

                UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), filter, Element.GetChangeTypeElementAddition());
            }

            return Result.Succeeded;

        }

    }


    public class ClashUpdater : IUpdater
    {
        UpdaterId _uid;
        public ClashUpdater(Guid guid)
        {
            _uid = new UpdaterId(new AddInId(new Guid("502fe383-2648-4e98-adf8-5e6047f9dc34")), // addin id
                guid); // updater id
        }

        public void Execute(UpdaterData data)
        {
            //Func<ICollection<ElementId>, string> toString = ids => ids.Aggregate("", (ss, id) => ss + "," + id).TrimStart(',');
            //var sb = new StringBuilder();
            //sb.AppendLine("added:" + toString(data.GetAddedElementIds()));
            //sb.AppendLine("modified:" + toString(data.GetModifiedElementIds()));
            //sb.AppendLine("deleted:" + toString(data.GetDeletedElementIds()));
            //TaskDialog.Show("Changes", sb.ToString());
            Document doc = data.GetDocument();
            Application app = doc.Application;
            foreach (ElementId id in data.GetAddedElementIds())
            {
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                //  TaskDialog.Show("ElevationWatcher Updater", string.Format("New elevation view '{0}'", data.GetAddedElementIds()));
                ElementIntersectsElementFilter EIEF = new ElementIntersectsElementFilter(doc.GetElement(id));
               
                //collector.WherePasses(EIEF);
                //ICollection<Element> allLoads = collector.ToElements();
                List<Element> Collisions = new List<Element>();

                IList<Element> elems = collector.OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkType)).ToElements();
                foreach (Element e in elems)
                {

                   

                    RevitLinkType linkType = e as RevitLinkType;

                    String s = String.Empty;



                    foreach (Document linkedDoc in app.Documents)

                    {
                     //   TaskDialog.Show("Collision tracker", string.Format("A {0}  // {1}", linkedDoc.Title + ".rvt", linkType.Name));

                        if (linkedDoc.Title+".rvt" ==(linkType.Name))
                        {


                            FilteredElementCollector collLinked = new FilteredElementCollector(linkedDoc);

                            collLinked.WherePasses(EIEF);

                           // Collisions=collLinked.ToElements();


                          //  TaskDialog.Show("Collision tracker", string.Format(" {0} ", Collisions.Count));
                    
                            foreach (Element el in collLinked.ToElements())
                            {
                                Collisions.Add(el);
                                TaskDialog.Show("ElevationWatcher Updater", string.Format("El Name: {0}  File: {1}", el.Name, linkedDoc.Title));
                            }

                        }
                    }

                }

                FilteredElementCollector _collector = new FilteredElementCollector(doc);
                _collector.WherePasses(EIEF);
                foreach (Element el in _collector.ToElements())
                {
                    Collisions.Add(el);
                    
                }

                if (Collisions.Count > 0)
                {
                    TaskDialog.Show("Collision tracker", string.Format("This path have '{0}' collisions.", Collisions.Count));

                }
            }
        }

        public string GetAdditionalInformation()
        {
            return "N/A";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FreeStandingComponents;
        }

        public UpdaterId GetUpdaterId()
        {
            return _uid;
        }

        public string GetUpdaterName()
        {
            return "ParameterUpdater";
        }

        private static void EnableUpdater()
        {

        }
    }

}
