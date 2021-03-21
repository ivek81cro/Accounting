using System.Windows;
using System.Windows.Controls;

namespace LoginModule.Components
{
    /// <summary>
    /// Interaction logic for PBoxBindable.xaml
    /// </summary>
    public partial class PBoxBindable : UserControl
    {

        private bool _isPasswordChanging;
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PBoxBindable),
                new PropertyMetadata(string.Empty, PasswordPropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PBoxBindable passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        public PBoxBindable()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = PasswordBox.Password;
            _isPasswordChanging = false;
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                PasswordBox.Password = Password;
            }
        }
    }
}
