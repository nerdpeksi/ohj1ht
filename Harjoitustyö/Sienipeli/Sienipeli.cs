using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;

namespace Sienipeli
{
    public class Sienipeli : PhysicsGame
    {
        public override void Begin()
        {
            // Kirjoita ohjelmakoodisi tähän

            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        }
    }
}