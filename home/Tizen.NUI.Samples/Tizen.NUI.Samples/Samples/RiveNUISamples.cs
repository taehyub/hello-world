using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.NUI.Samples
{
    public class RiveNUISmaple : IExample
    {
        
        string [] ICON_PATH = { 
        Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/RiveAnimationView/earth.png",
        Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/RiveAnimationView/moon.png",
        Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/RiveAnimationView/sun.png",
        Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/RiveAnimationView/jupiter.png",
        Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/RiveAnimationView/venus.png"
        };

        string [] TITLE_PATH = {
            "Earth", "Moon", "Sun", "Jupiter", "Venus",
        };

        Color [] TITLE_COLOR = {
            new Color(38.0f/255.0f, 169.0f/255.0f, 242.0f/255.0f, 1.0f),
            new Color(177.0f/255.0f, 177.0f/255.0f, 177.0f/255.0f, 1.0f),
            new Color(241.0f/255.0f, 173.0f/255.0f, 63.0f/255.0f, 1.0f),
            new Color(232.0f/255.0f, 130.0f/255.0f, 101.0f/255.0f, 1.0f),
            new Color(233.0f/255.0f, 221.0f/255.0f, 198.0f/255.0f, 1.0f),
        };

        string [] TEXT_PATH = {
        "Earth is the third planet from the sun.",
        "Moon is Earth's only natural satellite.",
        "Sun is the star at the center of the Solar System.",
        "Jupiter the fifth planet from the Sun.",
        "Venus is the second planet from the Sun."
        };

        string [] VIEW_BG_PATH = {
            Tizen.Applications.Application.Current.DirectoryInfo.Resource + "ViewItemBgImageFirst.png",
            Tizen.Applications.Application.Current.DirectoryInfo.Resource + "ViewItemBgImage.png",
            Tizen.Applications.Application.Current.DirectoryInfo.Resource + "ViewItemBgImage.png",
            Tizen.Applications.Application.Current.DirectoryInfo.Resource + "ViewItemBgImage.png",
            Tizen.Applications.Application.Current.DirectoryInfo.Resource + "ViewItemBgImageEnd.png"
        };

        string VIEW_CLICKED_BG = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "ViewItemBgImageFirstClicked.png";

        Size [] VIEW_SIZE_PATH = {
            new Size(720, 310),
            new Size(720, 300),
            new Size(720, 300),
            new Size(720, 300),
            new Size(720, 310),
        };

        private Window window;
        private Layer defaultLayer;
        private View scrollView;
        private Components.ScrollableBase scroll;
        private TextLabel[] verticalItems;
        private TextLabel header;
        private View[] viewItems;

        RiveAnimationView rav;
        Button playButton, stopButton;
        Button bounceButton, brokeButton;
        Button fillButton, strokeButton, opacityButton;
        Button scaleButton, rotationButton, positionButton;

        private float ravCenterY;
        private bool isFirst;
        private bool scrollMode;

        private Position startPos;
        private Position scrollViewPos;
        private float preScrollPositionY;

        public void Activate()
        {
            scrollMode = false;

            window = NUIApplication.GetDefaultWindow();
            defaultLayer = window.GetDefaultLayer();

            rav = new RiveAnimationView(Tizen.Applications.Application.Current.DirectoryInfo.Resource + "space_reload.riv")
            {
                Size = new Size(720, 500),
                //ParentOrigin = ParentOrigin.Center,
                //PivotPoint = PivotPoint.Center,
                Position = new Position(0, 72)
                //PositionUsesPivotPoint = true,
            };


            window.TouchEvent += OnRiveWindowTouchEvent;            

            rav.EnableAnimation("Idle", true);
            rav.EnableAnimation("Pull", true);
            rav.SetAnimationTimeMode("Pull",true);
            rav.PlayAnimation();

            scrollView = new View()
            {
                Size = new Size(720, 1160),
                Position = new Position(0, 120),
                BackgroundColor = new Color(01.0f, 0.0f, 0.0f, 0.0f),
            };

            scroll = new Components.ScrollableBase()
            {
                Size = new Size(720, 1160),
                ScrollingDirection = Components.ScrollableBase.Direction.Vertical,
                EnableOverShootingEffect = true,
                HideScrollbar = true,
            };
            
            scroll.ScrollEnabled = false;

            header = new TextLabel
            {
                FontFamily = "SamsungUI",
                Position = new Position(0, 0),
                Size = new Size(720, 120),
                BackgroundColor = new Color(52.0f/255.0f, 43.0f/255.0f, 117.0f/255.0f, 1.0f),
                Text = "Universe",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
                PointSize = 20.0f,  
            };

            viewItems = new View[10];
            for (int i = 0; i < 5; i++)
            {
                viewItems[i] = new View
                {
                    Position = new Position(0, i * 300),
                    Size = VIEW_SIZE_PATH[i],
                    BackgroundImage = VIEW_BG_PATH[i],
                    //BackgroundColor = new Color(52.0f/255.0f, 43.0f/255.0f, 117.0f/255.0f, 1.0f),
                    BackgroundImageBorder = new Rectangle(0, 0, 720, 300),
                    BorderlineWidth = 10,
                    BorderlineColor = new Color (0.0f, 0.0f, 0.0f, 1.0f),
                    Layout = new LinearLayout()
                    {
                        LinearOrientation = LinearLayout.Orientation.Vertical,
                        CellPadding = new Size(40, 0),
                        LinearAlignment = LinearLayout.Alignment.Center,
                    }
                };
                if (i==0)
                    viewItems[i].TouchEvent += OnRiveItemTouchEvent;            

                ImageView icon = new ImageView
                {
                    ResourceUrl = ICON_PATH[i],
                    Size = new Size(200, 200),
                    BackgroundColor = new Color(73.0f/255.0f, 68.0f/255.0f, 140.0f/255.0f, 1.0f),
                    HeightResizePolicy = ResizePolicyType.Fixed,
                    WidthResizePolicy = ResizePolicyType.Fixed,
                };

                TextLabel title = new TextLabel
                {
                    Size = new Size(550, 70),
                    PointSize = 12.0f,
                    //BackgroundColor = new Color(73.0f/255.0f, 68.0f/255.0f, 140.0f/255.0f, 1.0f),
                    MultiLine = true,
                    //Margin = new Extents(50, 50, 50, 50),
                    FontFamily = "SamsungUI",
                    Text = TITLE_PATH[i],
                    TextColor = TITLE_COLOR[i],
                    //가능
                    //BackgroundImage = ITEM_ICON_IMAGE,
                };

                TextLabel text = new TextLabel
                {
                    Size = new Size(550, 100),
                    PointSize = 9.0f,
                    //BackgroundColor = new Color(73.0f/255.0f, 68.0f/255.0f, 140.0f/255.0f, 1.0f),
                    MultiLine = true,
                    //Margin = new Extents(50, 50, 50, 50),
                    FontFamily = "SamsungUI",
                    Text = TEXT_PATH[i],
                    TextColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
                    //가능
                    //BackgroundImage = ITEM_ICON_IMAGE,
                };

                if (i % 2 == 0)
                {
                   // viewItems[i].BackgroundColor = Color.White;
                }
                else
                {
                   // viewItems[i].BackgroundColor = Color.Purple;
                }
                //viewItems[i].Add(icon);
                viewItems[i].Add(title);
                viewItems[i].Add(text);
                scroll.Add(viewItems[i]);
            }

            scroll.ScrollOutOfBound += OnVerticalScrollOutOfBound;
            scrollView.Add(scroll);
            defaultLayer.Add(rav);
            defaultLayer.Add(scrollView);
            defaultLayer.Add(header);
        }

        private void OnVerticalScrollOutOfBound(object sender, Components.ScrollOutOfBoundEventArgs e)
        {
            if (e.PanDirection == Components.ScrollOutOfBoundEventArgs.Direction.Up)
            {

            }
            else if (e.PanDirection == Components.ScrollOutOfBoundEventArgs.Direction.Down)
            {
            }
        }

        private bool OnRiveItemTouchEvent(object source, View.TouchEventArgs e)
        {
            PointStateType GetState = e.Touch.GetState(0);
            Random ran = new Random();
            double r = ran.NextDouble();
            double g = ran.NextDouble();
            double b = ran.NextDouble();
            
            if (GetState == PointStateType.Down)
            {
                viewItems[0].BackgroundImage = VIEW_CLICKED_BG;
                rav.SetShapeFillColor("EarthColor", new Color((float)r, (float)g, (float)b, 1.0f));
            }
            if (GetState == PointStateType.Up)
            {
                viewItems[0].BackgroundImage = VIEW_BG_PATH[0];
            }
            return false;
        }

        [global::System.Obsolete]
        private void OnRiveWindowTouchEvent(object source, Window.TouchEventArgs e)
        {
            Vector2 lp = e.Touch.GetLocalPosition(0);
            Vector2 sp = e.Touch.GetScreenPosition(0);
            PointStateType state = e.Touch.GetState(0);
            Position screenPos = new Position(sp.X, sp.Y);

            if (!scrollMode)
            {
            
            if (state == PointStateType.Down)
            {
                startPos = screenPos;
                scrollViewPos = scrollView.Position;
            }

            Position diff = screenPos - startPos;

            scrollView.PositionX = 0.0f;
            float minmaxY = scrollViewPos.Y + diff.Y;

            
            if (minmaxY > 520)
                scrollView.PositionY = 520;
            else if(minmaxY <= 120)
                scrollView.PositionY = 120;
            else
                scrollView.PositionY = minmaxY;
                
            float time = (scrollView.PositionY - 120 ) / 400;
            rav.SetAnimationTime("Pull", time);

            if (time < 1.0)
            {
                isFirst = true;
                rav.EnableAnimation("Trigger", false);
                rav.EnableAnimation("Loading", false);
            }
            else if (time >= 1.0 && isFirst)
            {
                isFirst = false;
                
                rav.EnableAnimation("Trigger", true);
                rav.EnableAnimation("Loading", true);
                
                scroll.ScrollEnabled = true;
                scrollMode = true;
                scroll.Size = new Size(720, 760);
                scrollView.Size = new Size(720, 760);
            }
            }
            else
            {
                if (preScrollPositionY == 0)
                {
                    float diff = scroll.ScrollPosition.Y - preScrollPositionY;
                    if (diff > 3)
                    {
                        scroll.ScrollEnabled = false;
                        scrollMode = false;
                        scroll.Size = new Size(720, 1160);
                        scrollView.Size = new Size(720, 1160);
                    }                    
                }
                preScrollPositionY = scroll.ScrollPosition.Y;
            }

//            Tizen.Log.Error("KTH", $"scrollView cur {scrollView.CurrentPosition.X} {scrollView.CurrentPosition.Y} {scrollView.PositionX} {scrollView.PositionY}");
            ravCenterY = (120.0f + scrollView.Position.Y) / 2;
            rav.PositionX = 0.0f;
            rav.PositionY = ravCenterY - 248.0f;
            //Tizen.Log.Error("KTH", $"scroll position {scroll.ScrollPosition.Y}");
        }

        public void Deactivate()
        {
            defaultLayer.Remove(rav);
            defaultLayer.Remove(playButton);
            defaultLayer.Remove(stopButton);
            defaultLayer.Remove(bounceButton);
            defaultLayer.Remove(brokeButton);
            defaultLayer.Remove(fillButton);
            defaultLayer.Remove(strokeButton);
            defaultLayer.Remove(opacityButton);
            defaultLayer.Remove(scaleButton);
            defaultLayer.Remove(rotationButton);
            defaultLayer.Remove(positionButton);
        }
    }
}
