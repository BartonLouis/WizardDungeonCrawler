using UnityEngine.UI;

namespace Louis.Patterns.EventSystem {
    public readonly struct MenuButtonPressedEvent : IEvent {
        public readonly string button;
        public MenuButtonPressedEvent(string button) => this.button = button;
    }

    public readonly struct MenuButtonSelectedEvent : IEvent {
        public readonly Button selectable;
        public MenuButtonSelectedEvent(Button selectable) => this.selectable = selectable;
    }
}