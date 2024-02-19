using System.Windows.Media;
using System.Windows.Ink;
using System.Windows;
using System.Windows.Controls;
using PaintGameMVVM.Services;
using System.Windows.Media.Imaging;
using PaintGameMVVM.Infrastructure.Converters;
using System.Linq;
using System.Collections.Generic;
using BaseClassesLyb;
using WpfBaseLyb;

namespace PaintGameMVVM.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {

        private ViewModelConverter _converter;

        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        private string _currentTheme;
        public string CurrentTheme
        {
            get => _currentTheme;

            set
            {
                _currentTheme = value;
                OnPropertyChanged(nameof(CurrentTheme));
            }
        }
        private AiPrediction DivineAi;


        #region атрибуты панели инструментов


        private Stack<StrokeCollection> strokeHistory = new Stack<StrokeCollection>();

        private StrokeCollection _canvasStrokeCollection;

        public StrokeCollection CanvasStrokeCollection
        {
            get { return _canvasStrokeCollection; }
            set
            {
                if (_canvasStrokeCollection != value)
                {
                    _canvasStrokeCollection = value;
                    OnPropertyChanged(nameof(CanvasStrokeCollection));
                }
            }
        }

        private InkCanvasEditingMode _editingMode;
        public InkCanvasEditingMode CanvasEditingMode
        {
            get => _editingMode;
            set
            {
                if (value != _editingMode)
                {
                    _editingMode = value;
                    OnPropertyChanged(nameof(CanvasEditingMode));
                }
            }
        }

        private int _currentSize = 4;
        public int CurrentSize 
        {
            get => _currentSize;

            set
            {
                if (value is int && value>0)
                {
                    _currentSize = value;
                    _customDrawingAttributes.Width = _currentSize;
                    _customDrawingAttributes.Height = _currentSize;
                }
            }
        }

        private DrawingAttributes _customDrawingAttributes;
        public DrawingAttributes CustomDrawingAttributes
        {
            get { return _customDrawingAttributes; }
            set
            {
                _customDrawingAttributes = value;
                OnPropertyChanged(nameof(CustomDrawingAttributes));
            }
        }

        private Color _currentColor;

        public Color CurrentColor 
        {
            get => _currentColor;

            set
            {
                _currentColor = value;
                CustomDrawingAttributes.Color = value;
                OnPropertyChanged(nameof(CurrentColor));
            }
        
        }
        #endregion
        #region Команды

        public CommandBase AiPredictCommand { get; }
        private bool CanAiPredictExecute(object p) => true;

        private void AiPredictExecute(object bytes)
        {
            
            if (bytes is InkCanvas canvas)
            {
                
                var res = DivineAi.CreatePrediction(_converter.InkCanvasToByteArr(canvas));

                if (res == CurrentTheme)
                {
                    MessageBox.Show($"Вы отлично справились с рисованием {_currentTheme}.");
                }
                else
                {
                    MessageBox.Show($"Боюсь вы не справились с рисованием {_currentTheme}. Я подумал, что это {res}");
                }
                CanvasStrokeCollection.Clear();
                strokeHistory.Clear();
                CurrentTheme = DivineAi.CreateNewTheme();
            }
        }
        #region Работа со строками канваса
        public CommandBase AddStroke { get; }

        private bool CanAddStrokeExecute(object p) => true;

        private void AddStrokeExecute(object canvasStrokes)
        {
            if (canvasStrokes is StrokeCollection _canvasStrokes)
            {
                StrokeCollection newStrokes = new StrokeCollection();
                foreach (Stroke stroke in _canvasStrokes)
                {
                    newStrokes.Add(stroke.Clone());
                }
                strokeHistory.Push(newStrokes);

            }
        }
        public SimpleCommand RemoveStroke { get; }

        private bool CanRemoveStrokeExecute() => CanvasStrokeCollection.Count > 0;

        private void RemoveStrokeExecute()
        {
            if (strokeHistory.Count > 1)
            {
                if(strokeHistory.Peek().Count() == CanvasStrokeCollection.Count)
                    strokeHistory.Pop();
                CanvasStrokeCollection = strokeHistory.Pop();
                OnPropertyChanged(nameof(CanvasStrokeCollection));
            }
            else
            {
                CanvasStrokeCollection.Clear();     
            }
        }
        #endregion

        public CommandBase ChangeEditingMode { get; }

        private bool CanChangeEditingModeExecute(object p) => true;

        private void ChangeEditingModeExecute(object parameter)
        {
            if (parameter is InkCanvasEditingMode)
            {
                CanvasEditingMode = (InkCanvasEditingMode)parameter;
            }
        }
        #endregion
        public MainWindowViewModel()
        {
            _converter = new ViewModelConverter();
            //команда для изменения кисти

            CustomDrawingAttributes = new DrawingAttributes
            {
                Color = Color.FromRgb(0, 0,0),
                Width = 4,
                Height = 4
            };
            //команда обновления списка строк
            AddStroke = new RegularCommand(AddStrokeExecute, CanAddStrokeExecute);
            RemoveStroke = new SimpleCommand(RemoveStrokeExecute, CanRemoveStrokeExecute);
            CanvasStrokeCollection = new StrokeCollection();
            strokeHistory.Push(new StrokeCollection());

            //команда для смены режима рисования
            ChangeEditingMode = new RegularCommand(ChangeEditingModeExecute, CanChangeEditingModeExecute);
            CanvasEditingMode = InkCanvasEditingMode.Ink;

            //инициализация ИИ
            DivineAi= new AiPrediction();
            CurrentTheme = DivineAi.CreateNewTheme();
            AiPredictCommand = new RegularCommand(AiPredictExecute, CanAiPredictExecute);

        }
    }
}