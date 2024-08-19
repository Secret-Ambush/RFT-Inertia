using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp1
{
    public class MyDocumentPaginator : DocumentPaginator
    {
        private List<Visual> _visuals;
        private Size _pageSize;
        private bool _isLandscape;

        public MyDocumentPaginator(List<Visual> visuals, Size pageSize, bool isLandscape)
        {
            _visuals = visuals ?? throw new ArgumentNullException(nameof(visuals));
            _pageSize = pageSize;
            _isLandscape = isLandscape;
        }

        public override bool IsPageCountValid => true;
        public override int PageCount => _visuals.Count;
        public override Size PageSize
        {
            get => _pageSize;
            set => throw new NotSupportedException();
        }

        public override IDocumentPaginatorSource Source => null;
        public override DocumentPage GetPage(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber >= PageCount)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), $"Invalid page number: {pageNumber}");

            var visual = _visuals[pageNumber];

            if (visual is FrameworkElement frameworkElement)
            {
                if (_isLandscape)
                {
                    var rotateTransform = new RotateTransform(90, _pageSize.Width / 2, _pageSize.Height / 2);
                    frameworkElement.RenderTransform = rotateTransform;
                }

                frameworkElement.Measure(_pageSize);
                frameworkElement.Arrange(new Rect(_pageSize));
            }

            return new DocumentPage(visual);
        }
    }
}
