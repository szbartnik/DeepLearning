namespace Wkiro.ImageClassification.Core.Models.Requests
{
    public class SkipPhaseRequest : PropertyChangedNotifier
    {
        private bool _requestedAndUnhandled;

        public bool RequestedAndUnhandled
        {
            get { return _requestedAndUnhandled; }
            set
            {
                _requestedAndUnhandled = value;
                OnPropertyChanged();
            }
        }
    }
}