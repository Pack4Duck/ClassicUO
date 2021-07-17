using ClassicUO.Configuration;
using ClassicUO.Game.Managers;
using ClassicUO.Game.UI.Controls;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using ClassicUO.Resources;

namespace ClassicUO.Game.UI.Gumps
{
    /// <summary>
    /// UI Manager
    /// - Reset Position of Gump on a screen
    /// </summary>
    internal sealed class UiManagerGump: Gump
    {
        private const ushort HUE_FONT = 0xFFFF;
        private const ushort BACKGROUND_COLOR = 999;
        private const ushort GUMP_WIDTH = 450;
        private const ushort GUMP_HEIGHT = 400;

        public UiManagerGump(): base(130, 130)
        {
            CanMove = true;

            Add
            (
                new AlphaBlendControl(0.05f)
                {
                    X = 1,
                    Y = 1,
                    Width = GUMP_WIDTH,
                    Height = GUMP_HEIGHT,
                    Hue = BACKGROUND_COLOR,
                    AcceptMouseInput = true,
                    CanMove = true,
                    CanCloseWithRightClick = true,
                }
            );
            #region Border Draw
            Add
            (
                new Line(0, 0, GUMP_WIDTH, 1, Color.Gray.PackedValue)
            );

            Add
            (
                new Line(0, 0, 1, GUMP_HEIGHT, Color.Gray.PackedValue)
            );

            Add
            (
                new Line(0, GUMP_HEIGHT, GUMP_WIDTH, 1, Color.Gray.PackedValue)
            );

            Add
            (
                new Line(GUMP_WIDTH, 0, 1, GUMP_HEIGHT, Color.Gray.PackedValue)
            );
            #endregion

            #region Legend
            Add(new Label(ResGumps.UIManagerGumpName, true, HUE_FONT, 0, 255, Renderer.FontStyle.BlackBorder) { X = 5, Y = 10 });
            Add(new Label("X", true, HUE_FONT, 0, 255, Renderer.FontStyle.BlackBorder) { X = 300, Y = 10 });
            Add(new Label("Y", true, HUE_FONT, 0, 255, Renderer.FontStyle.BlackBorder) { X = 340, Y = 10 });
            Add(new Label(ResGumps.UIManegerGumpReset, true, HUE_FONT, 0, 255, Renderer.FontStyle.BlackBorder) { X = 390, Y = 10 });
            Add(new Line(0, 30, GUMP_WIDTH, 1, Color.Gray.PackedValue));
            #endregion

            ScrollArea rightArea = new ScrollArea
            (
                10, 45, GUMP_WIDTH - 15, GUMP_HEIGHT - 60,
                true
            );
            
            var y = 0;
            // Add All Gumps that are not savable and Is Visible
            foreach (var gump in UIManager.Gumps.Where(x => x.CanBeSaved && x.IsVisible))
            {
                rightArea.Add(new UiManagerRecordControl(gump) { Y = y });
                y += 20;
            }
            Add(rightArea);
            SetInScreen();
        }

        private sealed class UiManagerRecordControl : Control
        {
            private readonly Gump _gump;

            public UiManagerRecordControl(Gump gump)
            {
                CanMove = true;
                CanCloseWithRightClick = true;
                AcceptMouseInput = true;

                _gump = gump;
                StringBuilder sb = new StringBuilder(gump.ToString().Split('.').Last());
                // If gump is Angorable Append text
                if(_gump is AnchorableGump aGump)
                {
                    switch (aGump.AnchorType) {
                        case ANCHOR_TYPE.SPELL:
                            sb.Append($" [{aGump.Tooltip}]");
                            break;
                        case ANCHOR_TYPE.SKILL:
                            sb.Append($" [{(aGump as SkillButtonGump)?.SkillName}]");
                            break;
                        case ANCHOR_TYPE.HEALTHBAR:
                            sb.Append($" [{(aGump as HealthBarGump)?.Name}]");
                            break;
                        case ANCHOR_TYPE.MACRO:
                            sb.Append($" [{(aGump as MacroButtonGump)?._macro.Name}]");
                            break;
                    }
                }
                //Gump Name
                Add(new Label(sb.ToString(), true, HUE_FONT, 290) { X = 10 });
                //Gump X
                Add(new Label(_gump.X.ToString(), true, HUE_FONT, 250) { X = 290 });
                //Gump Y
                Add(new Label(_gump.Y.ToString(), true, HUE_FONT, 250) { X = 330 });
                //Gump Reset button
                Add(new Button(1, 0xFAB, 0xFAC) { X = 380, ButtonAction = ButtonAction.Activate });
            }

            public override void OnButtonClick(int buttonId)
            {
                //Center of Game Window
                var x = ProfileManager.CurrentProfile.GameWindowSize.X >> 1;
                var y = ProfileManager.CurrentProfile.GameWindowSize.X >> 1;

                switch (buttonId)
                {
                    case 1:
                        if(_gump is AnchorableGump aGump)
                        {
                            // If AnchorableGump is anchored to another gump we need to Update Location of all anchored gumps
                            var aManager = UIManager.AnchorManager[aGump];
                            if (aManager != null)
                            {
                                aManager.UpdateLocation(this, -aGump.X + x, -aGump.Y + y);
                                return;
                            }
                        }
                        _gump.X = x;
                        _gump.Y = y;

                        break;
                }
            }
        }
    }
}
