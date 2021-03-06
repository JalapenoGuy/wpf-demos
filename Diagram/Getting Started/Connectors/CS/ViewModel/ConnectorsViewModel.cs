#region Copyright Syncfusion Inc. 2001-2020.
// Copyright Syncfusion Inc. 2001-2020. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Connectors.Utility;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.UI.Xaml.Diagram.Controls;
using Syncfusion.UI.Xaml.Diagram.Theming;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Connectors
{
    /// <summary>
    /// Represents the class which is used to render the connectors.
    /// </summary>
    public class ConnectorsViewModel: DiagramViewModel
    {
        #region Fields

        //Creating the command for selecting shapes
        public ICommand SelectShapeCommand { get; set; }

        //To hold the shape name of the buttons.
        public string ShapeName = "Rectangle";

        //To hold the shape button which is selected.
        public Button prevbutton = null;

        //To hold the list of decorators collection
        private List<Arrows> decorators = new List<Arrows>();

        //To hold the source decorator.
        private Arrows sourceDecorator = new Arrows() { LineData = "M0,0 z", Name = "None" };

        //To hold the target decorator.
        private Arrows targetDecorator = new Arrows() { LineData = App.Current.Resources["ClosedSharp"] as string, Name = "ClosedSharp" };

        //To hold the segment decorator.
        private Arrows segmentDecorator = new Arrows() { LineData = "M0,0 z", Name = "None" };

        //To hold the fill color value.
        private Brush fillcolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6f409f"));


        /// <summary>
        /// Gets or sets the decorations collection.
        /// </summary>
        public List<Arrows> Decorators
        {
            get
            {
                return decorators;
            }
        }

        /// <summary>
        /// Gets or sets the source decorator.
        /// </summary>
        public Arrows SourceDecorator
        {
            get
            {
                return sourceDecorator;
            }
            set
            {
                if (sourceDecorator != value)
                {
                    sourceDecorator = value;
                    OnSourceDecoratorsChanged(sourceDecorator.Name.ToString());
                    OnPropertyChanged("Fill");
                }
            }
        }

        /// <summary>
        /// Gets or sets the target decorators.
        /// </summary>
        public Arrows TargetDecorator
        {
            get
            {
                return targetDecorator;
            }
            set
            {
                if (targetDecorator != value)
                {
                    targetDecorator = value;
                    OnTargetDecoratorsChanged(targetDecorator.Name.ToString());
                    OnPropertyChanged("Fill");
                }
            }
        }

        /// <summary>
        /// Gets or sets the segment decorator.
        /// </summary>
        public Arrows SegmentDecorator
        {
            get
            {
                return segmentDecorator;
            }
            set
            {
                if (segmentDecorator != value)
                {
                    segmentDecorator = value;
                    OnSegmentDecoratorsChanged(segmentDecorator.Name.ToString());
                    OnPropertyChanged("Fill");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Fill color of the nodes and connectors. 
        /// </summary>
        public Brush Fillcolor
        {
            get
            {
                return fillcolor;
            }
            set
            {
                if (fillcolor != value)
                {
                    fillcolor = value;
                    OnPropertyChanged("Fillcolor");
                    OnFillColorChanged(fillcolor);
                }
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Created the new instances of the <see cref="ConnectorsViewModel"/> class.
        /// </summary>
        public ConnectorsViewModel()
        {
            //Initialize the nodes and connectors collection
            this.Nodes = new ObservableCollection<CustomNodeViewModel>();
            this.Connectors = new ObservableCollection<CustomConnectorViewModel>();

            //Initialize the port visibility as collapse
            this.PortVisibility = PortVisibility.Collapse;

            //Initialize the default connector type.
            this.DefaultConnectorType = ConnectorType.CubicBezier;

            //Enable the routing to the connectors.
            this.Constraints |= GraphConstraints.Routing;

            //Initialize the horizontal and vertical ruler.
            this.HorizontalRuler = new Ruler()
            {
                Orientation = Orientation.Horizontal,
            };
            this.VerticalRuler = new Ruler()
            {
                Orientation = Orientation.Vertical,
            };
            //Initialize the snap settings.
            this.SnapSettings = new SnapSettings()
            {
                SnapConstraints = SnapConstraints.ShowLines,
            };

            //Initialize the shape selection command.
            SelectShapeCommand = new Command(OnSelectShapeCommandExecute);

            //Initialize the item added command.
            ItemAddedCommand = new Command(OnItemAddedCommandExecute);

            //Initialize the view port chnaged command.
            ViewPortChangedCommand = new Command(OnViewPortChangedCommandExecute);

            //Create and add nodes
            CustomNodeViewModel promotion = CreateNodes(140, 350, 30, "Promotion");
            CustomNodeViewModel lead = CreateNodes(300, 350, 70, "Lead");
            CustomNodeViewModel account = CreateNodes(500, 270, 30, "Account");
            CustomNodeViewModel information = CreateNodes(500, 350, 30, "Information");
            CustomNodeViewModel opportunity = CreateNodes(500, 430, 30, "Opportunity");
            CustomNodeViewModel template = CreateNodes(700, 350, 204, "");
            template.ContentTemplate = App.Current.Resources["ContentTemplateforNodeContent"] as DataTemplate;

            //Create node ports
            CreateNodePort(promotion, "promotion", 1, 0.5);

            CreateNodePort(lead, "lead1", 0, 0.5);
            CreateNodePort(lead, "lead2", 1, 0.5);
            CreateNodePort(lead, "lead3", 1, 0.75);
            CreateNodePort(lead, "lead4", 1, 0.25);

            CreateNodePort(information, "information1", 0, 0.5);
            CreateNodePort(information, "information2", 1, 0.5);

            CreateNodePort(account, "account1", 0, 0.5);
            CreateNodePort(account, "account2", 1, 0.5);

            CreateNodePort(opportunity, "opportunity1", 0, 0.5);
            CreateNodePort(opportunity, "opportunity2", 1, 0.5);

            CreateNodePort(template, "template1", 0, 0.5);
            CreateNodePort(template, "template2", 0, 0.4);
            CreateNodePort(template, "template3", 0, 0.6);

            //Add nodes to Nodes property of the Diagram
            (this.Nodes as ObservableCollection<CustomNodeViewModel>).Add(promotion);
            (this.Nodes as ObservableCollection<CustomNodeViewModel>).Add(lead);
            (this.Nodes as ObservableCollection<CustomNodeViewModel>).Add(account);
            (this.Nodes as ObservableCollection<CustomNodeViewModel>).Add(information);
            (this.Nodes as ObservableCollection<CustomNodeViewModel>).Add(opportunity);
            (this.Nodes as ObservableCollection<CustomNodeViewModel>).Add(template);

            //Create and add connectors
            CreateConnectors("promotion", "lead1");
            CreateConnectors("lead4", "account1");
            CreateConnectors("lead2", "information1");
            CreateConnectors("lead3", "opportunity1");
            CreateConnectors("template2", "account2");
            CreateConnectors("template1", "information2");
            CreateConnectors("template3", "opportunity2");
            this.CreateDecoraorsCollection();

            this.SelectedItems = new SelectorViewModel();
            (this.SelectedItems as SelectorViewModel).SelectorConstraints &= ~(SelectorConstraints.QuickCommands);
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Create and add the decorators collection 
        /// </summary>
        private void CreateDecoraorsCollection()
        {
            decorators.Add(new Arrows() { LineData = "M0,0 z", Name = "None" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["Open90"] as string, Name = "Open90" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OpenFletch"] as string, Name = "OpenFletch" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["DimensionLine"] as string, Name = "DimensionLine" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedSharp"] as string, Name = "ClosedSharp" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["IndentedClosed"] as string, Name = "IndentedClosed" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OutdentedClosed"] as string, Name = "OutdentedClosed" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedFlench"] as string, Name = "ClosedFlench" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["BlackSlash"] as string, Name = "BlackSlash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["Diamond"] as string, Name = "Diamond" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["Ellipse"] as string, Name = "Ellipse" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["Rectangle"] as string, Name = "Rectangle" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OpenDoubleArrowSingleDash"] as string, Name = "OpenDoubleArrowSingleDash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OpenArrowSingleDash"] as string, Name = "OpenArrowSingleDash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OpenDouble"] as string, Name = "OpenDouble" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedDouble"] as string, Name = "ClosedDouble" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedThreeDash"] as string, Name = "ClosedThreeDash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedTwoDash"] as string, Name = "ClosedTwoDash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OpenOnedDash"] as string, Name = "OpenOnedDash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["OpenTwoDash"] as string, Name = "OpenTwoDash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["Open3Dash"] as string, Name = "Open3Dash" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["Fork"] as string, Name = "Fork" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedFork"] as string, Name = "ClosedFork" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedPlus"] as string, Name = "ClosedPlus" });
            decorators.Add(new Arrows() { LineData = App.Current.Resources["ClosedOneDash"] as string, Name = "ClosedOneDash" });
        }

        /// <summary>
        /// To change the corner radious property values to the orthogonal connector.
        /// </summary>
        /// <param name="parameter"></param>
        private void OnCornerRadiousCommandExecute(object parameter)
        {
            if (this != null)
            {
                if ((bool)parameter)
                {
                    foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                    {
                        connector.CornerRadius = 10;
                    }
                }
                else
                {
                    foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                    {
                        connector.CornerRadius = 0;
                    }
                }
            }
        }

        /// <summary>
        /// To enable or disable the free hand drawing value to the tools property.
        /// </summary>
        /// <param name="parameter">command parameter value</param>
        private void OnFreeHandDrawingCommandExecute(object parameter)
        {
            if (this != null)
            {
                if ((bool)parameter)
                {
                    this.Tool = Tool.ContinuesDraw;
                    this.DrawingTool = DrawingTool.FreeHand;
                }
                else
                {
                    this.Tool = Tool.MultipleSelect;
                    this.DrawingTool = DrawingTool.Connector;
                }
            }
        }

        /// <summary>
        /// Command to bring the objects into center of the viewport.
        /// </summary>
        /// <param name="parameter">Command parameter value</param>
        private void OnViewPortChangedCommandExecute(object parameter)
        {
            if (this.Info != null)
            {
                (this.Info as IGraphInfo).BringIntoCenter((parameter as ChangeEventArgs<object, ScrollChanged>).NewValue.ContentBounds);
            }
        }

        /// <summary>
        /// Command to execute item added event.
        /// </summary>
        /// <param name="parameter">Command parameter value.</param>
        private void OnItemAddedCommandExecute(object parameter)
        {
            if ((parameter as ItemAddedEventArgs).Item is CustomConnectorViewModel)
            {
                ((parameter as ItemAddedEventArgs).Item as CustomConnectorViewModel).Fill = Fillcolor;
            }
        }

        /// <summary>
        /// To change the source decorator value of the connector.
        /// </summary>
        /// <param name="value">Source decorator shape name</param>
        private void OnSourceDecoratorsChanged(string value)
        {
            foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
            {
                connector.SourceDecorator = App.Current.Resources[value];
            }
        }

        /// <summary>
        /// To change the target decorator shape to the connectors.
        /// </summary>
        /// <param name="value">Target decorator shape name</param>
        private void OnTargetDecoratorsChanged(string value)
        {
            foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
            {
                connector.TargetDecorator = App.Current.Resources[value];
            }
        }

        /// <summary>
        /// To change the fill color of connector pathsand its decorators, Node stroke color.
        /// </summary>
        /// <param name="fillcolor"></param>
        private void OnFillColorChanged(Brush fillcolor)
        {
            Brush value = fillcolor;
            foreach (INode node in this.Nodes as ObservableCollection<CustomNodeViewModel>)
            {
                (node as CustomNodeViewModel).Stroke = value;
            }
            foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
            {
                (connector as CustomConnectorViewModel).Fill = value;
            }
        }

        /// <summary>
        /// To change the segment decorator shape to the connectors.
        /// </summary>
        /// <param name="value">Segment decorator shape name</param>
        private void OnSegmentDecoratorsChanged(string value)
        {
            foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
            {
                connector.SegmentDecorators = new ObservableCollection<ISegmentDecorator>()
                {
                    //Define the SegmentDecorator
                     new SegmentDecorator()
                     {
                        Shape = App.Current.Resources[value] as object,
                        Length = 0.5,
                    }
                };
            }
        }

        /// <summary>
        /// To change the connector types from the property panel buttons.
        /// </summary>
        /// <param name="parameter">Command paramter value.</param>
        private void OnSelectShapeCommandExecute(object parameter)
        {
            Button button = parameter as Button;
            this.ShapeName = button.Name.ToString();

            if (prevbutton != null)
            {
                prevbutton.Style = Application.Current.Resources["ButtonStyle"] as Style;
            }

            button.Style = Application.Current.Resources["SelectedButtonStyle"] as Style;
            if (ShapeName.Equals("StraightConnector"))
            {
               foreach(IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new StraightSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 1;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.Line;
            }
            else if (ShapeName.Equals("OrthogonalConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new OrthogonalSegment()
                        {
                        }
                    };

                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 1;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.Orthogonal;
                this.OnCornerRadiousCommandExecute(false);
            }
            else if (ShapeName.Equals("BezierConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new CubicCurveSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 1;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.CubicBezier;
            }
            else if (ShapeName.Equals("ThickStraightConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new StraightSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = App.Current.Resources["Ellipse"];
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.Line;
            }
            else if (ShapeName.Equals("ThickOrthogonalConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new OrthogonalSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = App.Current.Resources["Ellipse"];
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.Orthogonal;
                this.OnCornerRadiousCommandExecute(false);
            }
            else if (ShapeName.Equals("ThickBezierConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new CubicCurveSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = App.Current.Resources["Ellipse"];
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.CubicBezier;
            }
            else if (ShapeName.Equals("DashStraightConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new StraightSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = 2;
                }

                this.DefaultConnectorType = ConnectorType.Line;
            }
            else if (ShapeName.Equals("DashOrthogonalConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new OrthogonalSegment()
                        {
                        }
                    };

                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = 2;
                }

                this.DefaultConnectorType = ConnectorType.Orthogonal;
                this.OnCornerRadiousCommandExecute(false);
            }
            else if (ShapeName.Equals("DashBezierConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new CubicCurveSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = 2;
                }

                this.DefaultConnectorType = ConnectorType.CubicBezier;
            }
            else if (ShapeName.Equals("CornerRediousConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new OrthogonalSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = null;
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = double.MaxValue;
                }

                this.DefaultConnectorType = ConnectorType.Orthogonal;
                this.OnCornerRadiousCommandExecute(true);
            }
            else if (ShapeName.Equals("DashEllipseConnector"))
            {
                foreach (IConnector connector in this.Connectors as ObservableCollection<CustomConnectorViewModel>)
                {
                    connector.Segments = new ObservableCollection<IConnectorSegment>()
                    {
                        new StraightSegment()
                        {
                        }
                    };
                    connector.SourceDecorator = App.Current.Resources["Ellipse"];
                    (connector as CustomConnectorViewModel).StrokeThickess = 2;
                    (connector as CustomConnectorViewModel).StrokeDash = 2;
                }

                this.DefaultConnectorType = ConnectorType.Line;
            }

            prevbutton = parameter as Button;
        }

        /// <summary>
        /// Create and return the node.
        /// </summary>
        /// <param name="offsetx">offset-x of the node</param>
        /// <param name="offsety">offset-y of the node.</param>
        /// <param name="height">Height of the node</param>
        /// <param name="text">Text annotation of the node</param>
        /// <returns></returns>
        private CustomNodeViewModel CreateNodes(double offsetx, double offsety, double height, string text)
        {
            CustomNodeViewModel node = new CustomNodeViewModel()
            {
                UnitHeight = height,
                UnitWidth = 100,
                OffsetX = offsetx,
                OffsetY = offsety,
                Shape = App.Current.Resources["RoundedRectangle"],
                Fill = new SolidColorBrush(Colors.White),
                Annotations = new ObservableCollection<IAnnotation>()
                {
                    new TextAnnotationViewModel()
                    {
                        Text = text,
                        FontSize = 13,
                        Foreground = new SolidColorBrush(Colors.Black),
                        FontFamily = new FontFamily("Calibri(Body)"),
                    }
                },
            };

            return node;
        }

        /// <summary>
        /// Create the node ports
        /// </summary>
        /// <param name="node1">Node</param>
        /// <param name="id">Port ID</param>
        /// <param name="nodeoffsetx">Node offset-x value.</param>
        /// <param name="nodeoffsety">Node offset-y value.</param>
        private void CreateNodePort(CustomNodeViewModel node1, string id, double nodeoffsetx, double nodeoffsety)
        {
            NodePortViewModel nodePort = new NodePortViewModel()
            {
                ID = id,
                NodeOffsetX = nodeoffsetx,
                NodeOffsetY = nodeoffsety,
            };
          (node1.Ports as PortCollection).Add(nodePort);
        }

        /// <summary>
        /// To create and add the connectors.
        /// </summary>
        /// <param name="sourceid">Source Port ID</param>
        /// <param name="targetid">Target port ID</param>
        private void CreateConnectors(string sourceid, string targetid)
        {
            CustomConnectorViewModel connector = new CustomConnectorViewModel()
            {
                SourcePortID = sourceid,
                TargetPortID = targetid,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6f409f")),
            };

            //Add Node to Nodes property of the Diagram
            (this.Connectors as ObservableCollection<CustomConnectorViewModel>).Add(connector);
        }
        #endregion
    }

    /// <summary>
    /// Class to customize the <see cref="NodeViewModel"/> class.
    /// </summary>
    public class CustomNodeViewModel : NodeViewModel
    {

        #region Fields

        //To hold the fill color of the nodes.
        private Brush fill = new SolidColorBrush(Colors.Black);

        //To hold the stroke color the nodes.
        private Brush stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6f409f"));

        /// <summary>
        /// Gets or sets the fill color of the node.
        /// </summary>
        public Brush Fill
        {
            get
            {
                return fill;
            }
            set
            {
                if (fill != value)
                {
                    fill = value;
                    OnPropertyChanged("Fill");
                }
            }
        }

        //Gets or sets stroke color of the nodes.
        public Brush Stroke
        {
            get
            {
                return stroke;
            }
            set
            {
                if (stroke != value)
                {
                    stroke = value;
                    OnPropertyChanged("Fill");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// To change the fill color, stroke of the nodes.
        /// </summary>
        private void OnFillChanged()
        {
            Style s = new Style(typeof(Path));
            double val = 2;
            if (Fill != null)
            {
                s.Setters.Add(new Setter(Path.FillProperty, new SolidColorBrush(Colors.White)));
                s.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));
                s.Setters.Add(new Setter(Path.StrokeProperty, Stroke));
                s.Setters.Add(new Setter(Path.StrokeThicknessProperty, val));
            }
            ShapeStyle = s;
        }

        /// <summary>
        /// To change the property values.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        protected override void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);

            switch (name)
            {
                case "Fill":
                    OnFillChanged();
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Create the custom connector of the <see cref="ConnectorViewModel"/> class.
    /// </summary>
    public class CustomConnectorViewModel : ConnectorViewModel
    {

        #region Fields

        //To hold the fill color the connectors.
        private Brush fill = new SolidColorBrush(Colors.Black);

        //To hold the stroke thickness value of the connectors.
        private double strokeThickess = 2;

        //To hold the stroke dash property value of the connectors.
        private double strokeDash = double.MaxValue;

        /// <summary>
        /// Gets or sets the fill color to connectors  
        /// </summary>
        public Brush Fill
        {
            get
            {
                return fill;
            }
            set
            {

                if (fill != value)
                {
                    fill = value;
                    OnPropertyChanged("Fill");
                }
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness value to the connectors
        /// </summary>
        public double StrokeThickess
        {
            get
            {
                return strokeThickess;
            }
            set
            {
                if (strokeThickess != value)
                {
                    strokeThickess = value;
                    OnPropertyChanged("Fill");
                }
            }
        }

        /// <summary>
        /// Gets or sets the stroke dash values to the connectors.
        /// </summary>
        public double StrokeDash
        {
            get
            {
                return strokeDash;
            }
            set
            {
                if (strokeDash != value)
                {
                    strokeDash = value;
                    OnPropertyChanged("Fill");
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// To change the fill color to the connectors.
        /// </summary>
        private void OnFillChanged()
        {
            Style connectorGeometryStyle = new Style(typeof(Path));
            Style sourceDecoratorStyle = new Style(typeof(Path));
            Style targetDecoratorStyle = new Style(typeof(Path));
            Style segmentDecoratorStyle = new Style(typeof(Path));

            if (Fill != null)
            {
                connectorGeometryStyle.Setters.Add(new Setter(Path.StrokeProperty, Fill));
                connectorGeometryStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, StrokeThickess));
                connectorGeometryStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, new DoubleCollection() { StrokeDash }));

                targetDecoratorStyle.Setters.Add(new Setter(Path.StrokeProperty, Fill));
                targetDecoratorStyle.Setters.Add(new Setter(Path.FillProperty, Fill));
                targetDecoratorStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, StrokeThickess));
                targetDecoratorStyle.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));
                targetDecoratorStyle.Setters.Add(new Setter(Path.StrokeLineJoinProperty, PenLineJoin.Round));
                targetDecoratorStyle.Setters.Add(new Setter(Path.StrokeStartLineCapProperty, PenLineCap.Round));
                targetDecoratorStyle.Setters.Add(new Setter(Path.StrokeEndLineCapProperty, PenLineCap.Round));
                targetDecoratorStyle.Setters.Add(new Setter(Path.StrokeDashCapProperty, PenLineCap.Round));
                targetDecoratorStyle.Setters.Add(new Setter(Path.WidthProperty, 15d));
                targetDecoratorStyle.Setters.Add(new Setter(Path.HeightProperty, 12d));

                sourceDecoratorStyle.Setters.Add(new Setter(Path.StrokeProperty, Fill));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.FillProperty, Fill));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, StrokeThickess));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.StrokeLineJoinProperty, PenLineJoin.Round));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.StrokeStartLineCapProperty, PenLineCap.Round));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.StrokeEndLineCapProperty, PenLineCap.Round));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.StrokeDashCapProperty, PenLineCap.Round));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.WidthProperty, 15d));
                sourceDecoratorStyle.Setters.Add(new Setter(Path.HeightProperty, 12d));

                segmentDecoratorStyle.Setters.Add(new Setter(Path.StrokeProperty, Fill));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.FillProperty, Fill));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, StrokeThickess));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.StretchProperty, Stretch.Fill));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.StrokeLineJoinProperty, PenLineJoin.Round));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.StrokeStartLineCapProperty, PenLineCap.Round));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.StrokeEndLineCapProperty, PenLineCap.Round));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.StrokeDashCapProperty, PenLineCap.Round));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.WidthProperty, 15d));
                segmentDecoratorStyle.Setters.Add(new Setter(Path.HeightProperty, 12d));
            }

            ConnectorGeometryStyle = connectorGeometryStyle;
            TargetDecoratorStyle = targetDecoratorStyle;
            SourceDecoratorStyle = sourceDecoratorStyle;
            SegmentDecoratorStyle = segmentDecoratorStyle;
        }

        protected override void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);

            switch (name)
            {
                case "Fill":
                    OnFillChanged();
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Class to create the list of decorators collections 
    /// </summary>
    public class Arrows
    {
        /// <summary>
        /// Gets or sets the path data of the decorators.
        /// </summary>
        public string LineData { get; set; }

        /// <summary>
        /// Gets or sets the decorator path name
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Class to convert the colors to the brush value.
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var converter = new System.Windows.Media.BrushConverter();

                Brush brush = (Brush)converter.ConvertFromString(value.ToString());

                return (brush as SolidColorBrush).Color;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(value.ToString());
                return brush;
            }
            return value;
        }
    }
}
