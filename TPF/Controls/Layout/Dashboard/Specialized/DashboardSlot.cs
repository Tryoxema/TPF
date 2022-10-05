namespace TPF.Controls.Specialized.Dashboard
{
    internal class DashboardSlot
    {
        public DashboardSlot(Widget widget)
        {
            Top = widget.Top;
            Left = widget.Left;
            HorizontalSlots = widget.HorizontalSlots;
            VerticalSlots = widget.VerticalSlots;
        }

        public DashboardSlot(int top, int left, int horizontalSlots, int verticalSlots)
        {
            Top = top;
            Left = left;
            HorizontalSlots = horizontalSlots;
            VerticalSlots = verticalSlots;
        }

        public int Top { get; set; }

        public int Left { get; set; }

        public int HorizontalSlots { get; set; }

        public int VerticalSlots { get; set; }

        public int Bottom
        {
            get { return Top + VerticalSlots - 1; }
        }

        public int Right
        {
            get { return Left + HorizontalSlots - 1; }
        }
    }
}