﻿using System;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedImage : Image
    {
        private TapGestureRecognizer _tapGestureRecognizer;

        private void CreateOrRemoveGestureRecognizer()
        {
            if (!IsSharable && Command == null)
            {
                if (_tapGestureRecognizer == null) return;

                GestureRecognizers.Remove(_tapGestureRecognizer);
                _tapGestureRecognizer = null;
            }
            else
            {
                if (_tapGestureRecognizer != null) return;

                _tapGestureRecognizer = new TapGestureRecognizer
                {
                    Command = new RelayCommand(p =>
                    {
                        if (Command != null)
                            Command.Execute(CommandParameter ?? p);

                        OnClicked();
                    }, p => Command == null || Command.CanExecute(CommandParameter ?? p))
                };

                GestureRecognizers.Add(_tapGestureRecognizer);
            }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<ExtendedImage, ICommand>(p => p.Command, null,
            propertyChanged: CommandPropertyChanged);

        private static void CommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var command = oldvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged -= ((ExtendedImage) bindable).CommandOnCanExecuteChanged;

            command = newvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged += ((ExtendedImage) bindable).CommandOnCanExecuteChanged;

            ((ExtendedImage) bindable).CreateOrRemoveGestureRecognizer();
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            if (_tapGestureRecognizer != null && _tapGestureRecognizer.Command != null)
                ((RelayCommand) _tapGestureRecognizer.Command).RaiseCanExecuteChanged();
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<ExtendedImage, object>(p => p.CommandParameter, null,
             propertyChanged: CommandParameterPropertyChanged);
        
        private static void CommandParameterPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var gesture = ((ExtendedImage) bindable)._tapGestureRecognizer;

            if (gesture != null && gesture.Command != null)
                ((RelayCommand)gesture.Command).RaiseCanExecuteChanged();
        }

        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create<ExtendedImage, Color>(p => p.TintColor, Color.Default);

        public static readonly BindableProperty IsSharableProperty =
            BindableProperty.Create<ExtendedImage, bool>(p => p.IsSharable, false,
            propertyChanged: IsSharablePropertyChanged);

        private static void IsSharablePropertyChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            ((ExtendedImage)bindable).CreateOrRemoveGestureRecognizer();
        }

        public static readonly BindableProperty ShareTextProperty =
            BindableProperty.Create<ExtendedImage, string>(p => p.ShareText, string.Empty);

        public static readonly BindableProperty ImageLabelTextProperty =
            BindableProperty.Create<ExtendedImage, string>(p => p.ImageLabelText, string.Empty);

        public static readonly BindableProperty LabelBackgroundColorProperty =
            BindableProperty.Create<ExtendedImage, Color>(p => p.LabelBackgroundColor, new Color(0, 0, 0, 0.75));

        public static readonly BindableProperty LabelColorProperty =
            BindableProperty.Create<ExtendedImage, Color>(p => p.LabelColor, Color.Black);

        public static readonly BindableProperty CircularProperty =
            BindableProperty.Create<ExtendedImage, bool>(p => p.Circular, false);

        public static readonly BindableProperty CircleBorderColorProperty =
            BindableProperty.Create<ExtendedImage, Color>(p => p.CircleBorderColor, Color.Black);

        public static readonly BindableProperty CircleBorderWidthProperty =
            BindableProperty.Create<ExtendedImage, int>(p => p.CircleBorderWidth, 0);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public bool IsSharable
        {
            get { return (bool)GetValue(IsSharableProperty); }
            set { SetValue(IsSharableProperty, value); }
        }

        public string ShareText
        {
            get { return (string)GetValue(ShareTextProperty); }
            set { SetValue(ShareTextProperty, value); }
        }

        public string ImageLabelText
        {
            get { return (string)GetValue(ImageLabelTextProperty); }
            set { SetValue(ImageLabelTextProperty, value); }
        }

        public Color LabelBackgroundColor
        {
            get { return (Color)GetValue(LabelBackgroundColorProperty); }
            set { SetValue(LabelBackgroundColorProperty, value); }
        }

        public Color LabelColor
        {
            get { return (Color)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }

        public bool Circular
        {
            get { return (bool)GetValue(CircularProperty); }
            set { SetValue(CircularProperty, value); }
        }

        public Color CircleBorderColor
        {
            get { return (Color)GetValue(CircleBorderColorProperty); }
            set { SetValue(CircleBorderColorProperty, value); }
        }

        public int CircleBorderWidth
        {
            get { return (int)GetValue(CircleBorderWidthProperty); }
            set { SetValue(CircleBorderWidthProperty, value); }
        }

        public event EventHandler Clicked;

        private void OnClicked()
        {
            var handler = Clicked;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
