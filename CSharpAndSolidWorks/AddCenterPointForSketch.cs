﻿using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpAndSolidWorks
{
    public class AddCenterPointForSketch
    {
        public ISldWorks _swApp;
        public ModelDoc2 _swPartModel;
        public ModelDoc2 _swModel;

        public AddCenterPointForSketch()
        {
            _swApp = Utility.ConnectToSolidWorks();

            _swPartModel = _swApp.ActiveDoc;

            _swModel = _swApp.ActiveDoc;
        }

        private void CreateHeaterCenter(IEnumerable<string> heaterPathSketches)
        {
            ISketchPoint startPoint = null;
            ISketchPoint endPoint = null;
            List<SegmentData> list = new List<SegmentData>();
            List<SegmentData> list2 = new List<SegmentData>();
            List<SegmentData> list3 = new List<SegmentData>();
            List<SegmentData> list4 = new List<SegmentData>();
            _swApp.ActivateDoc(_swPartModel.GetTitle());
            _swPartModel.ShowNamedView2("*Front", 1);
            foreach (string str in heaterPathSketches)
            {
                ISketchArc arc;
                double length;
                list2.Clear();
                list.Clear();
                _swPartModel.Extension.SelectByID2(str, "SKETCH", 0.0, 0.0, 0.0, true, 0, null, 0);
                _swPartModel.ViewZoomToSelection();
                ISelectionMgr iSelectionManager = _swPartModel.ISelectionManager;

                IFeature feature = iSelectionManager.GetSelectedObject6(1, 0);

                ISketch sketch = feature.GetSpecificFeature2();

                Array array = sketch.GetSketchSegments();

                Array array2 = sketch.GetSketchPoints2();
                foreach (ISketchSegment segment in array)
                {
                    ISketchSegment segment2 = segment;
                    segment2.Select(true);
                    if (!segment2.ConstructionGeometry)
                    {
                        if (segment2.GetType() == 0)
                        {
                            ISketchLine line = (ISketchLine)segment2;
                            startPoint = line.IGetStartPoint2();
                            endPoint = line.IGetEndPoint2();
                        }
                        if (segment2.GetType() == 1)
                        {
                            arc = (ISketchArc)segment2;
                            startPoint = arc.IGetStartPoint2();
                            endPoint = arc.IGetEndPoint2();
                        }
                        SegmentData item = new SegmentData(segment2, startPoint, endPoint);
                        list.Add(item);
                    }
                    _swPartModel.ClearSelection2(true);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int num9 = 0;
                    list2.Add(list[0]);
                    list.Remove(list[0]);
                    for (int m = 0; m < list.Count; m++)
                    {
                        list2[0].Segment.Select(false);
                        list[m].Segment.Select(false);
                        if ((Math.Round(list2[0].StartPoint.X, 5) == Math.Round(list[m].StartPoint.X, 5)) && (Math.Round(list2[0].StartPoint.Y, 5) == Math.Round(list[m].StartPoint.Y, 5)))
                        {
                            num9++;
                        }
                        if ((Math.Round(list2[0].EndPoint.X, 5) == Math.Round(list[m].EndPoint.X, 5)) && (Math.Round(list2[0].EndPoint.Y, 5) == Math.Round(list[m].EndPoint.Y, 5)))
                        {
                            num9++;
                        }
                        if ((Math.Round(list2[0].EndPoint.X, 5) == Math.Round(list[m].StartPoint.X, 5)) && (Math.Round(list2[0].EndPoint.Y, 5) == Math.Round(list[m].StartPoint.Y, 5)))
                        {
                            num9++;
                        }
                        if ((Math.Round(list2[0].StartPoint.X, 5) == Math.Round(list[m].EndPoint.X, 5)) && (Math.Round(list2[0].StartPoint.Y, 5) == Math.Round(list[m].EndPoint.Y, 5)))
                        {
                            num9++;
                        }
                        _swPartModel.ClearSelection2(true);
                        if (num9 == 2)
                        {
                            list.Add(list2[0]);
                            list2.Remove(list2[0]);
                            break;
                        }
                    }
                    if (num9 == 1)
                    {
                        list2[0].Segment.Select(true);
                        break;
                    }
                }
                int num = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    list2[num].Segment.Select(true);
                    list[j].Segment.Select(false);
                    if ((Math.Round(list2[num].StartPoint.X, 5) == Math.Round(list[j].StartPoint.X, 5)) && (Math.Round(list2[num].StartPoint.Y, 5) == Math.Round(list[j].StartPoint.Y, 5)))
                    {
                        list2.Add(list[j]);
                        list.Remove(list[j]);
                        num++;
                        j = -1;
                    }
                    else if ((Math.Round(list2[num].EndPoint.X, 5) == Math.Round(list[j].EndPoint.X, 5)) && (Math.Round(list2[num].EndPoint.Y, 5) == Math.Round(list[j].EndPoint.Y, 5)))
                    {
                        list2.Add(list[j]);
                        list.Remove(list[j]);
                        num++;
                        j = -1;
                    }
                    else if ((Math.Round(list2[num].EndPoint.X, 5) == Math.Round(list[j].StartPoint.X, 5)) && (Math.Round(list2[num].EndPoint.Y, 5) == Math.Round(list[j].StartPoint.Y, 5)))
                    {
                        list2.Add(list[j]);
                        list.Remove(list[j]);
                        num++;
                        j = -1;
                    }
                    else if ((Math.Round(list2[num].StartPoint.X, 5) == Math.Round(list[j].EndPoint.X, 5)) && (Math.Round(list2[num].StartPoint.Y, 5) == Math.Round(list[j].EndPoint.Y, 5)))
                    {
                        list2.Add(list[j]);
                        list.Remove(list[j]);
                        num++;
                        j = -1;
                    }
                }
                _swPartModel.ClearSelection();
                _swPartModel.Extension.SelectByID2(str, "SKETCH", 0.0, 0.0, 0.0, false, 0, null, 0);
                _swPartModel.EditSketch();
                foreach (ISketchPoint point3 in array2)
                {
                    point3.Select(false);
                    if (point3.Type == 1)
                    {
                        _swPartModel.EditDelete();
                    }
                }
                double num2 = 0.0;
                foreach (SegmentData data2 in list2)
                {
                    data2.Segment.Select(false);
                    length = data2.Segment.GetLength();
                    num2 += length;
                }
                _swPartModel.ClearSelection();
                double num4 = num2 / 2.0;
                length = 0.0;
                int num5 = 0;
                double num6 = 0.0;
                for (int k = 0; k < list2.Count; k++)
                {
                    list2[k].Segment.Select(false);
                    length += list2[k].Segment.GetLength();
                    if (num4 < length)
                    {
                        num5 = k;
                        double num7 = Math.Abs((double)(length - num4));
                        num6 = (list2[num5].Segment.GetLength() - num7) / list2[num5].Segment.GetLength();
                        break;
                    }
                }
                _swPartModel.ClearSelection();
                if (list2[num5].Segment.GetType() == 0)
                {
                    double num13 = 0.0;
                    double x = list2[num5].StartPoint.X;
                    double y = list2[num5].StartPoint.Y;
                    double num16 = list2[num5].EndPoint.X;
                    double num17 = list2[num5].EndPoint.Y;
                    if ((Math.Round(num16, 6) == Math.Round(list2[num5 - 1].StartPoint.X, 6)) && (Math.Round(num17, 6) == Math.Round(list2[num5 - 1].StartPoint.Y, 6)))
                    {
                        num16 = list2[num5].StartPoint.X;
                        num17 = list2[num5].StartPoint.Y;
                        x = list2[num5].EndPoint.X;
                        y = list2[num5].EndPoint.Y;
                    }
                    else if ((Math.Round(num16, 6) == Math.Round(list2[num5 - 1].EndPoint.X, 6)) && (Math.Round(num17, 6) == Math.Round(list2[num5 - 1].EndPoint.Y, 6)))
                    {
                        num16 = list2[num5].StartPoint.X;
                        num17 = list2[num5].StartPoint.Y;
                        x = list2[num5].EndPoint.X;
                        y = list2[num5].EndPoint.Y;
                    }
                    double pointX = x + ((num16 - x) * num6);
                    double pointY = y + ((num17 - y) * num6);
                    _swPartModel.SetAddToDB(true);
                    if (_swPartModel.CreatePoint2(pointX, pointY, 0.0) != null)
                    {
                        _swPartModel.SketchAddConstraints("sgFixed");
                    }
                    if (x == num16)
                    {
                        num13 = 1.5707963267948966;
                    }
                    if (y == num17)
                    {
                        num13 = 3.1415926535897931;
                    }
                    if ((x != num16) && !(y == num17))
                    {
                        num13 = Math.Atan((num17 - y) / (num16 - x));
                    }
                    if (num13 > 3.1415926535897931)
                    {
                        num13 -= 3.1415926535897931;
                    }
                    ISketchSegment segment3 = _swPartModel.ICreateLine2(pointX + (0.005 * Math.Cos(num13 + 1.5707963267948966)), pointY + (0.005 * Math.Sin(num13 + 1.5707963267948966)), 0.0, pointX + (0.005 * Math.Cos(num13 + 4.71238898038469)), pointY + (0.005 * Math.Sin(num13 + 4.71238898038469)), 0.0);
                    _swPartModel.SketchAddConstraints("sgFIXED");
                    segment3.ConstructionGeometry = true;
                    _swPartModel.SetAddToDB(false);
                    _swPartModel.ClearSelection2(true);
                    _swPartModel.InsertSketch2(true);
                }
                if (list2[num5].Segment.GetType() == 1)
                {
                    arc = (ISketchArc)list2[num5].Segment;
                    startPoint = arc.IGetStartPoint2();
                    endPoint = arc.IGetEndPoint2();
                    ISketchPoint point4 = arc.IGetCenterPoint2();
                    int rotationDir = arc.GetRotationDir();
                    double num21 = 0.0;
                    double num22 = 0.0;
                    double num23 = list2[num5].Segment.GetLength();
                    double radius = arc.GetRadius();
                    double num25 = (num6 * num23) / radius;
                    double num26 = startPoint.X;
                    double num27 = startPoint.Y;
                    double num28 = endPoint.X;
                    double num29 = endPoint.Y;
                    double num30 = point4.X;
                    double num31 = point4.Y;
                    if ((Math.Round(num28, 6) == Math.Round(list2[num5 - 1].StartPoint.X, 6)) && (Math.Round(num29, 6) == Math.Round(list2[num5 - 1].StartPoint.Y, 6)))
                    {
                        num26 = endPoint.X;
                        num27 = endPoint.Y;
                        rotationDir = -1 * rotationDir;
                    }
                    else if ((Math.Round(num28, 6) == Math.Round(list2[num5 - 1].EndPoint.X, 6)) && (Math.Round(num29, 6) == Math.Round(list2[num5 - 1].EndPoint.Y, 6)))
                    {
                        num26 = endPoint.X;
                        num27 = endPoint.Y;
                        rotationDir = -1 * rotationDir;
                    }
                    if ((num30 != num26) && !(num31 == num27))
                    {
                        num21 = Math.Atan((num31 - num27) / (num30 - num26));
                    }
                    if (Math.Round(num30, 8) == Math.Round(num26, 8))
                    {
                        if (Math.Round(num31, 8) > Math.Round(num27, 8))
                        {
                            num21 = 4.71238898038469;
                        }
                        else if (Math.Round(num31, 8) < Math.Round(num27, 8))
                        {
                            num21 = 1.5707963267948966;
                        }
                    }
                    else if (Math.Round(num31, 8) == Math.Round(num27, 8))
                    {
                        if (Math.Round(num30, 8) > Math.Round(num26, 8))
                        {
                            num21 = 3.1415926535897931;
                        }
                        else if (Math.Round(num30, 8) < Math.Round(num26, 8))
                        {
                            if (rotationDir > 0)
                            {
                                num21 = 0.0;
                            }
                            else
                            {
                                num21 = 6.2831853071795862;
                            }
                        }
                    }
                    if ((Math.Round(num30, 8) < Math.Round(num26, 8)) && (Math.Round(num31, 8) < Math.Round(num27, 8)))
                    {
                        num22 = num21 + (rotationDir * num25);
                    }
                    else if ((Math.Round(num30, 8) < Math.Round(num26, 8)) && (Math.Round(num31, 8) > Math.Round(num27, 8)))
                    {
                        num22 = (6.2831853071795862 + num21) + (rotationDir * num25);
                    }
                    else if ((Math.Round(num30, 8) > Math.Round(num26, 8)) && (Math.Round(num31, 8) > Math.Round(num27, 8)))
                    {
                        num22 = (3.1415926535897931 + num21) + (rotationDir * num25);
                    }
                    else if ((Math.Round(num30, 8) > Math.Round(num26, 8)) && (Math.Round(num31, 8) < Math.Round(num27, 8)))
                    {
                        num22 = (3.1415926535897931 + num21) + (rotationDir * num25);
                    }
                    else if (Math.Round(num30, 8) == Math.Round(num26, 8))
                    {
                        num22 = num21 + (rotationDir * num25);
                    }
                    else if (Math.Round(num31, 8) == Math.Round(num27, 8))
                    {
                        num22 = num21 + (rotationDir * num25);
                    }
                    num22 = Math.Round(num22, 6);
                    double num32 = num30 + (radius * Math.Cos(num22));
                    double num33 = num31 + (radius * Math.Sin(num22));
                    bool flag29 = true;
                    if (_swPartModel.Extension.SelectByID2("", "SKETCHPOINT", num32, num33, 0.0, false, 0, null, 0))
                    {
                        ISketchPoint point5 = iSelectionManager.GetSelectedObject6(0, 1);

                        if ((point5 != null) && ((Math.Round(point5.X, 6) == Math.Round(num32, 6)) && (Math.Round(point5.Y, 6) == Math.Round(num33, 6))))
                        {
                            flag29 = false;
                        }
                    }
                    _swPartModel.SetAddToDB(true);
                    if (flag29 && (_swPartModel.CreatePoint2(num32, num33, 0.0) != null))
                    {
                        _swPartModel.SketchAddConstraints("sgFIXED");
                        _swPartModel.ICreateLine2(num30, num31, 0.0, num32, num33, 0.0).ConstructionGeometry = true;
                    }
                    _swPartModel.SetAddToDB(false);
                    _swPartModel.ClearSelection2(true);
                    _swPartModel.InsertSketch2(true);
                }
            }
            // _swApp.ActivateDoc(_swDrawingModel.GetTitle());
        }

        public void CreateHeaterCL()
        {
            List<string> HeaterNamelist = new List<string>();
            List<string> heaterPathSketches = new List<string>();
            IFeatureManager featureManager = _swPartModel.FeatureManager;
            //获取当前零件所有特征
            object[] objArray = featureManager.GetFeatures(false);
            bool flag = false;
            //如果存在HeaterCL
            foreach (IFeature feature in objArray)
            {
                if (!feature.IsSuppressed() && (feature.Name == "HeaterCL"))
                {
                    flag = true;
                    _swPartModel.Extension.SelectByID("HeaterCL", "SKETCH", 0.0, 0.0, 0.0, true, 0, null);
                    _swPartModel.BlankSketch();
                    break;
                }
            }

            if (!flag)
            {
                foreach (IFeature feature3 in objArray)
                {
                    if (!feature3.IsSuppressed() && (feature3.GetTypeName2() == "SweepCut"))
                    {
                        IFeature feature4 = feature3.IGetFirstSubFeature().IGetNextSubFeature();
                        HeaterNamelist.Add(feature4.Name);
                        if (!feature3.Name.ToLower().Contains("th65") & !feature3.Name.ToLower().Contains("th85"))
                        {
                            heaterPathSketches.Add(feature4.Name);
                        }
                    }
                }

                if (HeaterNamelist.Count == 0)
                {
                    MessageBox.Show("No heater features found! Can't create HeaterCL sketch!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    CreateHeaterCenter(heaterPathSketches);

                    this._swPartModel.Extension.SelectByID2("Plane1", "PLANE", 0.0, 0.0, 0.0, true, 0, null, 0);
                    this._swPartModel.SketchManager.InsertSketch(true);
                    this._swPartModel.ClearSelection2(true);
                    foreach (string str in HeaterNamelist)
                    {
                        this._swPartModel.Extension.SelectByID2(str, "SKETCH", 0.0, 0.0, 0.0, true, 0, null, 0);
                    }
                    this._swPartModel.SketchUseEdge2(true);
                    this._swPartModel.ClearSelection2(true);
                    foreach (string str2 in heaterPathSketches)
                    {
                        this._swPartModel.Extension.SelectByID2(str2, "SKETCH", 0.0, 0.0, 0.0, true, 0, null, 0);
                        ISelectionMgr iSelectionManager = this._swPartModel.ISelectionManager;

                        IFeature feature5 = iSelectionManager.GetSelectedObject6(1, 0);

                        ISketch sketch2 = feature5.GetSpecificFeature2();

                        object[] objArray4 = sketch2.GetSketchPoints2();

                        Array array = sketch2.GetSketchSegments();
                        this._swPartModel.ClearSelection2(true);
                        foreach (ISketchSegment segment in array)
                        {
                            ISketchSegment segment2 = segment;
                            if (segment2.ConstructionGeometry)
                            {
                                segment2.Select(false);
                                this._swPartModel.SketchUseEdge2(true);
                                ISketch sketch3 = this._swPartModel.IGetActiveSketch2();

                                object[] objArray5 = sketch3.GetSketchSegments();
                                ISketchSegment segment3 = (ISketchSegment)objArray5[objArray5.Length - 1];
                                segment3.ConstructionGeometry = true;
                                this._swPartModel.ClearSelection2(true);
                            }
                        }
                        foreach (ISketchPoint point in objArray4)
                        {
                            if (point.Type == 1)
                            {
                                point.Select(true);
                                double x = point.X;
                                double y = point.Y;
                                SketchPoint point2 = _swPartModel.SketchManager.CreatePoint(x, y, 0.0);
                                if (point2 != null)
                                {
                                    point2.Select(true);
                                    point.Select(true);
                                    this._swPartModel.SketchAddConstraints("sgCOINCIDENT");
                                }
                                this._swPartModel.ClearSelection2(true);
                            }
                        }
                    }
                    _swPartModel.SketchManager.InsertSketch(true);
                    _swPartModel.IFeatureByPositionReverse(0).Select2(true, 0);
                    _swPartModel.SelectedFeatureProperties(0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, false, false, "HeaterCL");
                    _swPartModel.EditRebuild3();
                    _swPartModel.ViewZoomtofit();
                    _swPartModel.Extension.SelectByID("HeaterCL", "SKETCH", 0.0, 0.0, 0.0, true, 0, null);
                    _swPartModel.EditSketch();
                    ISketch sketch = this._swPartModel.IGetActiveSketch();

                    double[] numArray = sketch.GetArcs2();
                    int arcCount = sketch.GetArcCount();
                    _swModel.SetAddToDB(true);
                    for (int i = 0; i < arcCount; i++)
                    {
                        double num8 = numArray[(i * 0x10) + 6];
                        double num9 = numArray[(i * 0x10) + 7];
                        double num10 = numArray[(i * 0x10) + 9];
                        double num11 = numArray[(i * 0x10) + 10];
                        double num12 = numArray[(i * 0x10) + 12];
                        double num13 = numArray[(i * 0x10) + 13];
                        this._swPartModel.ICreateLine2(num12, num13, 0.0, num8, num9, 0.0).ConstructionGeometry = true;
                        this._swPartModel.ICreateLine2(num12, num13, 0.0, num10, num11, 0.0).ConstructionGeometry = true;
                    }
                    _swPartModel.SketchManager.InsertSketch(true);
                    _swModel.SetAddToDB(false);
                    _swPartModel.Extension.SelectByID("HeaterCL", "SKETCH", 0.0, 0.0, 0.0, true, 0, null);
                    _swPartModel.BlankSketch();
                    _swPartModel.EditRebuild3();
                    _swPartModel.SaveSilent();
                }
            }
        }
    }

    public class SegmentData
    {
        public SegmentData(ISketchSegment Segment, ISketchPoint StartPoint, ISketchPoint EndPoint)
        {
            this.Segment = Segment;
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;
        }

        public ISketchPoint EndPoint { get; set; }

        public ISketchSegment Segment { get; set; }

        public ISketchPoint StartPoint { get; set; }
    }
}