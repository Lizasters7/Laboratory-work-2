namespace Dormitory;

public partial class MainPage : ContentPage
{
    private const string Any = "— будь-який —";

    private readonly XmlSearcher _searcher = new(new SaxSearchStrategy());
    private readonly HtmlTransformer _transformer = new();

    private string? _xmlPath;
    private string? _xslPath;
    private List<Student>? _lastResults;   

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnPickXmlClicked(object? sender, EventArgs e)
    {
        string? path = await PickFileAsync(".xml", "Оберіть XML-файл");
        if (path == null) return;

        try
        {
            AttributeValues values = AttributeValues.ReadFromFile(path);
            FillPicker(FacultyPicker, values.Faculties);
            FillPicker(SpecialtyPicker, values.Specialties);
            FillPicker(CoursePicker, values.Courses);
            FillPicker(BuildingPicker, values.Buildings);

            _xmlPath = path;
            _lastResults = null;
            XmlFileLabel.Text = Path.GetFileName(path);
            OutputEditor.Text = "Файл завантажено. Оберіть параметри і натисніть «Пошук».";
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Помилка", $"Не вдалося прочитати XML:\n{ex.Message}", "OK");
        }
    }

    private async void OnPickXslClicked(object? sender, EventArgs e)
    {
        string? path = await PickFileAsync(".xsl", "Оберіть XSL-файл");
        if (path == null) return;
        _xslPath = path;
        XslFileLabel.Text = Path.GetFileName(path);
    }

    private static async Task<string?> PickFileAsync(string extension, string title)
    {
        var options = new PickOptions
        {
            PickerTitle = title,
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { extension } }
            })
        };
        FileResult? file = await FilePicker.Default.PickAsync(options);
        return file?.FullPath;
    }

    private static void FillPicker(Picker picker, List<string> values)
    {
        var items = new List<string> { Any };
        items.AddRange(values);
        picker.ItemsSource = items;
        picker.SelectedIndex = 0;
    }

    private static string? SelectedValue(Picker picker)
    {
        if (picker.SelectedItem is string s && s != Any)
            return s;
        return null;
    }

    private async void OnSearchClicked(object? sender, EventArgs e)
    {
        if (_xmlPath == null)
        {
            await DisplayAlertAsync("Увага", "Спочатку оберіть XML-файл.", "OK");
            return;
        }

        try
        {
            _searcher.SetStrategy(CreateSelectedStrategy());
            SearchCriteria criteria = ReadCriteria();
            _lastResults = _searcher.Search(_xmlPath, criteria);
            OutputEditor.Text = ResultFormatter.Format(_searcher.Strategy, criteria, _lastResults);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Помилка пошуку", ex.Message, "OK");
        }
    }

    private IXmlSearchStrategy CreateSelectedStrategy()
    {
        if (DomRadio.IsChecked) return new DomSearchStrategy();
        if (LinqRadio.IsChecked) return new LinqSearchStrategy();
        return new SaxSearchStrategy();
    }

    private SearchCriteria ReadCriteria()
    {
        return new SearchCriteria
        {
            Keyword = KeywordEntry.Text,
            Faculty = SelectedValue(FacultyPicker),
            Specialty = SelectedValue(SpecialtyPicker),
            Course = SelectedValue(CoursePicker),
            Building = SelectedValue(BuildingPicker)
        };
    }

    private async void OnTransformClicked(object? sender, EventArgs e)
    {
        if (_xmlPath == null || _xslPath == null)
        {
            await DisplayAlertAsync("Увага", "Оберіть і XML-, і XSL-файл.", "OK");
            return;
        }

        try
        {
            string folder = Path.GetDirectoryName(_xmlPath)!;
            string source = _xmlPath;

            if (_lastResults != null)
            {
                source = Path.Combine(folder, "search_results.xml");
                HtmlTransformer.SaveStudentsAsXml(_lastResults, source);
            }

            string htmlPath = Path.Combine(folder, Path.GetFileNameWithoutExtension(_xmlPath) + ".html");
            string html = _transformer.Transform(source, _xslPath, htmlPath);

            HtmlView.Source = new HtmlWebViewSource { Html = html };
            OutputEditor.Text += $"\nHTML збережено у файл: {htmlPath}";
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Помилка трансформації", ex.Message, "OK");
        }
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        OutputEditor.Text = string.Empty;
        KeywordEntry.Text = string.Empty;
        foreach (Picker p in new[] { FacultyPicker, SpecialtyPicker, CoursePicker, BuildingPicker })
            if (p.ItemsSource != null) p.SelectedIndex = 0;
        SaxRadio.IsChecked = true;
        HtmlView.Source = new HtmlWebViewSource { Html = string.Empty };
        _lastResults = null;
    }

    private async void OnExitClicked(object? sender, EventArgs e)
    {
        bool answer = await DisplayAlertAsync("Підтвердження",
            "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
        if (answer)
            Application.Current?.Quit();
    }
}