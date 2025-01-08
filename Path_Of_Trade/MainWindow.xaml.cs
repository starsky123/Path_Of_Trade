using Newtonsoft.Json;
using Path_Of_Trade.Item;
using Path_Of_Trade.Json;
using Path_Of_Trade.TradeAPI;
using Path_Of_Trade.TradeAPI.POE2;
using Path_Of_Trade.TradeAPI.POE2.FetchResult;
using Path_Of_Trade.TradeAPI.POE2.POEDictionary;
using Path_Of_Trade.TradeAPI.POE2.Query;
using Path_Of_Trade.TradeAPI.POE2.Query.QueryFilter.TradeFilter;
using Path_Of_Trade.TradeAPI.POE2.Query.StatFilter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Path_Of_Trade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool b = true;
        public System.Windows.Point be = new();
        public System.Windows.Point af = new();
        public static Dictionary<string, uint> keycode = new() { { "Ctrl", 0x0002 }, { "Alt", 0x0001 }, {"Shift", 0x0004 },{"Win", 0x0008 } };
        public static Dictionary<string, string> grid = new() { { "selected", "选择" }, { "label", "标签" }, { "value", "值" }, { "text", "词缀" }, { "type", "类型" } };
        public MainWindow()
        {
            InitializeComponent();
            try
            {               
                InitSettings_be();
                InitDictionary();
                InitSettings_af();
                
            }
            catch (Exception er) { messageshow(er.Message); }
        }

        public void InitLanguageCombo()
        {
            taglangcombo.Text = Settings1.Default.taglanguage;
            statslangcombo.Text = Settings1.Default.statslanguage;
        }
        public void InitLeagueCombo()
        {
            string result = SendHTTP.Get(SendHTTP.LeagueApi[1], 15);
            if (!string.IsNullOrEmpty(result)) 
            {
                League list=JsonConvert.DeserializeObject<League>(result);
                leaguecombo.ItemsSource = list.result;
                leaguecombo.DisplayMemberPath = "id";
            }
            leaguecombo.Text = Settings1.Default.league;
        }
        public void InitCurrencyCombo()
        {
            List<Currency> list = new List<Currency>() {
                new Currency("Any") ,
                new Currency("Chaos Orb") ,
                new Currency("Mirror of Kalandra") ,
                new Currency("Exalted Orb") ,
                new Currency("Divine Orb")
            };
            currencycombo.ItemsSource = list;
            currencycombo.DisplayMemberPath = "name_TR";
            currencycombo.SelectedValuePath = "name_EN";
            currencycombo.SelectedIndex = 0;
        }
        public void InitHotKeyCombo()
        {
            for (int i = 65; i <= 90; i++)
            { 
                char c= (char)i;
                keycombo.Items.Add(c);
            }
            mokeycombo.Text = Settings1.Default.mokey;
            keycombo.Text= Settings1.Default.key;
        }

        public bool InitSettings_be()
        {
            try
            {
                InitLanguageCombo();
                InitLeagueCombo();
                InitHotKeyCombo();
                fetchnud.Value = Settings1.Default.fetchnum;
                minnud.Value = Settings1.Default.minroll;
                maxnud.Value = Settings1.Default.maxroll;
                typecheckbox.Content = "";
                
                return true;
            }
            catch(Exception er) {messageshow(er.Message); return false; }
            
        }
        public bool InitDictionary()
        {
            try
            {
                ItemStats_Dictionary.SetItemStatsDictionary();
                Trade_Dictionary.SetTradeDictionary();
                Translate_Dictionary.Set(Settings1.Default.taglanguage);
                return true;
            }
            catch (Exception er) { messageshow(er.Message); return false; }
        }
        public bool InitSettings_af()
        {
            try
            {
                InitCurrencyCombo();
                corruptedcheckbox.Content = Translate_Dictionary.translate.FirstOrDefault(s => s.Value == "ItemPopupCorrupted").Key;
                return true;
            }
            catch (Exception er) { messageshow(er.Message); return false; }
        }
        public void SetWindowSize()
        {
            this.Height *= SystemParameters.PrimaryScreenHeight / (1600 / 1.5);
            this.Width *= SystemParameters.PrimaryScreenWidth / (2560 / 1.5);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings1.Default.fetchnum = (int)fetchnud.Value;
                Settings1.Default.maxroll= (int)maxnud.Value;
                Settings1.Default.minroll= (int)minnud.Value;
                Settings1.Default.taglanguage = taglangcombo.Text;
                Settings1.Default.statslanguage = statslangcombo.Text;
                Settings1.Default.league = leaguecombo.Text;
                if (UnregisterHotKey(_windowHandle, HOTKEY_ID))
                {
                    Settings1.Default.mokey = mokeycombo.Text;
                    Settings1.Default.key = keycombo.Text;
                    if (!RegisterHotKey(_windowHandle, HOTKEY_ID, keycode[Settings1.Default.mokey], Encoding.ASCII.GetBytes(Settings1.Default.key)[0]))
                        messageshow("不能注册此快捷键");
                    else
                    {
                        Settings1.Default.Save();
                        messageshow("保存成功");
                    }
                }
                
            }
            catch (Exception er){ messageshow(er.Message); }
        }
        public void ShowItemInfo(Equipment equipment)
        {
            clear();
            nametextbox.Text = equipment.name ?? "";
            typecheckbox.Content= equipment.type ?? "";
            tagdatagrid.ItemsSource = null;
            tagdatagrid.ItemsSource = equipment.Itemlabel;

            equipment.Itemlabel[0].selected= true;
            equipment.Itemlabel[1].selected= true;
            if(equipment.Itemlabel[1].value==LabelTranslate("ItemDisplayStringUnique"))
                typecheckbox.IsChecked=true;
            statsdatagrid.ItemsSource = null;
            statsdatagrid.ItemsSource = equipment.Itemstats;

            corruptedcheckbox.IsChecked = equipment.corrupted;

            
        }
        public void trade()
        {
            try
            {
                string league = Settings1.Default.league;
                string statslang = Settings1.Default.statslanguage;
                string name = nametextbox.Text;
                string type =((bool)typecheckbox.IsChecked)?typecheckbox.Content.ToString():"";
                int num = Settings1.Default.fetchnum;
                bool currupted =(bool) corruptedcheckbox.IsChecked;
                string currency=currencycombo.SelectedValue.ToString();
                bool Collapse= (bool)collapsecheckbox.IsChecked;
                TradeFilters_Filter tradeFilters = new TradeFilters_Filter();
                if (currency != "Any")
                {
                    tradeFilters.price = new() { option = currency };
                }
                if (Collapse)
                    tradeFilters.collapse = new("true");
                SearchItem srjson = SearchItem.Search(league, statslang, name, type, currupted,tagdatagrid, statsdatagrid, tradeFilters);
                //&& srjson.Total > 0
                if (srjson != null )
                {
                    webtextbox.Text = SendHTTP.SearchUrl[1] + Settings1.Default.league + @"/" + srjson.ID;
                    Fetch f = Fetch.FetchItem(srjson, num);
                    pricedatagrid.ItemsSource = null;
                    if (f != null)
                    {
                        List<ShowFetch> list = new();
                        foreach (Fetch_Result p in f.result)
                        {
                            list.Add(new ShowFetch()
                            {
                                price = p.listing.price.amount.ToString(),
                                currency = fetchresult_currency(p.listing.price.currency),
                                time = fetchresult_date(p.listing.indexed),
                                json = JsonConvert.SerializeObject(p)
                            });
                        }
                        pricedatagrid.ItemsSource = list;
                    }
                    else
                        messageshow("无查询结果");               
                }
            }
            catch (Exception er) { messageshow(er.Message); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (tagdatagrid.ItemsSource != null)
                trade();

        }

        public string fetchresult_date(string str)
        {
            DateTime now= DateTime.Now.ToUniversalTime();
            DateTime item = Convert.ToDateTime(str).ToUniversalTime();
            TimeSpan time = now - item;
            string days = time.Days == 0 ? "" : (time.Days.ToString() + "天 ");
            string hours= time.Hours == 0 ? "" : (time.Hours.ToString() + "小时 ");
            string minutes = time.Minutes == 0 ? "" : (time.Minutes.ToString() + "分 ");
            string seconds = time.Seconds == 0 ? "" : (time.Seconds.ToString() + "秒 ");
            return days + hours + minutes + seconds + "前";
        }
        public string fetchresult_currency(string str)
        {
            string name_EN = Trade_Dictionary.TradeStaticDictionary.FirstOrDefault(s => s.Value == str).Key;
            string name_TR = Translate_Dictionary.currency[name_EN];
            return name_TR;
        }

        private void tagdatagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (tagdatagrid.ItemsSource != null)
                if (e.AddedCells[0].Column.GetType() == typeof(DataGridCheckBoxColumn))
                {
                    DataGridCellInfo cellinfo = e.AddedCells[0];
                    CheckBox checkBox = cellinfo.Column.GetCellContent(cellinfo.Item) as CheckBox;
                    checkBox.IsChecked = !checkBox.IsChecked;
                    ItemLabel label = (ItemLabel)cellinfo.Item;
                    List<ItemLabel> itemLabel = tagdatagrid.ItemsSource as List<ItemLabel>;
                    itemLabel.SingleOrDefault(s => s.标签 == label.标签).选择 = !label.选择;
                }
        }

        private void statsdatagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (statsdatagrid.ItemsSource != null)
                if (e.AddedCells[0].Column.GetType() == typeof(DataGridCheckBoxColumn))
                {

                    DataGridCellInfo cellinfo = e.AddedCells[0];
                    CheckBox checkBox = cellinfo.Column.GetCellContent(cellinfo.Item) as CheckBox;
                    checkBox.IsChecked = !checkBox.IsChecked;
                    ItemStats stats = (ItemStats)cellinfo.Item;
                    List<ItemStats> itemStats = statsdatagrid.ItemsSource as List<ItemStats>;
                    itemStats.SingleOrDefault(s => s.类型 == stats.类型 && s.词缀 == stats.词缀).选择 = !stats.选择;
                    


                }
        }

        public void clear()
        {
            try
            {
                nametextbox.Text = "";
                typecheckbox.Content = "";
                typecheckbox.IsChecked = false;
                corruptedcheckbox.IsChecked = false;
                tagdatagrid.ItemsSource = null;
                statsdatagrid.ItemsSource = null;
                pricedatagrid.ItemsSource = null;
                webtextbox.Text = "";
                iteminfotextbox.Document = new();
            }
            catch(Exception ex) { messageshow(ex.Message); }
        }
        #region 注册全局快捷键
        //引入Winows API
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9527;

        //Modifiers:
        
        private IntPtr _windowHandle;
        private HwndSource _source;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
            if (!RegisterHotKey(_windowHandle, HOTKEY_ID, keycode[Settings1.Default.mokey], Encoding.ASCII.GetBytes(Settings1.Default.key)[0]))
                messageshow("不能注册此快捷键");
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;//热键消息
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            //int vkey = (((int)lParam >> 16) & 0xFFFF);
                            //if (vkey == Encoding.ASCII.GetBytes(Settings1.Default.key)[0])
                            //{
                            if (IsPOEActive())
                            {
                                Clipboard.Clear();
                                if (SimulateHotkey())
                                {
                                    if (Clipboard.ContainsText())
                                    {
                                        string clipboardText = Clipboard.GetText();
                                        Equipment equipment = new Equipment();
                                        if (equipment.GetItemInfo(clipboardText))
                                        {
                                            try
                                            {
                                                SendTo(this, HWND_TOPMOST);
                                                //this.Topmost = true;
                                                this.WindowState = System.Windows.WindowState.Normal;
                                                //this.Topmost = false;
                                                tabcontrol.SelectedIndex = 1;
                                            }
                                            catch (Exception er){ messageshow(er.Message); }
                                            ShowItemInfo(equipment);
                                            //SendTo(this, HWND_NOTOPMOST);

                                        }
                                    }
                                }
                            }
                                
                            //}
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
        
        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }
        #endregion

        #region 模拟Ctrl+C


        public static bool SimulateHotkey()
        {
            System.Windows.Forms.SendKeys.SendWait("^c");

            return true;
        }
        #endregion

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            minwindow(b);
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        public void minwindow(bool b)
        {
            if (b)
            {
                //this.Topmost = false;
                SendTo(this, HWND_BOTTOM);
                //this.WindowState = System.Windows.WindowState.Minimized;
            }
        }
        public static void messageshow(string str)
        {
            b = false;
            MessageBox.Show(str);
            b = true;
        }

        public bool IsPOEActive()
        {
            bool b = false;
            string processName = "PathOfExile";
            Process[] prc = Process.GetProcesses();
            foreach (Process pr in prc)
            {
                if (pr.ProcessName.Contains(processName))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        #region back
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_SHOWWINDOW = 0x0040;
        public static void SendTo(Window window, IntPtr t)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hwnd, t, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

        }
        #endregion



        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            be = e.GetPosition(this); 
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            af = e.GetPosition(this);
            this.Left += af.X - be.X;
            this.Top += af.Y - be.Y;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string url=webtextbox.Text;
            try
            {
                Process.Start(new ProcessStartInfo { UseShellExecute=true,FileName=url});
            }
            catch(Exception ex) { messageshow(ex.Message); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (InitSettings_be() && InitDictionary() && InitSettings_af())
                messageshow("重新加载成功");
            else messageshow("重新加载失败");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                iteminfotextbox.Document = new();
                List<ShowFetch> list = (List<ShowFetch>)pricedatagrid.ItemsSource;
                Fetch_Result fetchitem = JsonConvert.DeserializeObject<Fetch_Result>(list[pricedatagrid.SelectedIndex].json);
                string iteminfo = "";
                if (fetchitem.item.name != "")
                {
                    if (fetchitem.item.rarity == "Unique")
                        iteminfo += LabelTranslate(fetchitem.item.name)+" ";
                    else
                    {
                        string[] temp = fetchitem.item.name.Split(' ');
                        iteminfo += LabelTranslate(temp[0]) + LabelTranslate(" " + temp[1]) + " ";
                    }
                }
                iteminfo += LabelTranslate(fetchitem.item.baseType) + "\r\n";
                iteminfo += "--------\r\n";
                iteminfo += LabelTranslate("ItemDisplayStringRarity") + ": " + LabelTranslate("ItemDisplayString"+fetchitem.item.rarity) + "\r\n";
                iteminfo += LabelTranslate("ItemDisplayStringItemLevel") + ": " + fetchitem.item.ilvl.ToString() + "\r\n";
                iteminfo += "--------\r\n";
                iteminfo += ModsTranslate(fetchitem.item.runeMods);
                iteminfo += "--------\r\n";
                iteminfo += ModsTranslate(fetchitem.item.implicitMods);
                iteminfo += "--------\r\n";
                iteminfo += ModsTranslate(fetchitem.item.explicitMods);
                iteminfo += "--------\r\n";
                if (fetchitem.item.corrupted)
                {
                    iteminfo += LabelTranslate("ItemPopupCorrupted") + "\r\n";
                    iteminfo += "--------\r\n";
                }
                iteminfo += "卖家账号: " + fetchitem.listing.account.name + "\r\n";
                iteminfo += "最后登录角色: " + fetchitem.listing.account.lastCharacterName + "\r\n";
                iteminfo += "--------\r\n";
                iteminfo += fetchitem.listing.whisper + "\r\n";
                iteminfotextbox.AppendText(iteminfo.Replace("--------\r\n--------\r\n", "--------\r\n"));
            }
            catch (Exception er) { messageshow(er.Message); }
        }
        public string StatsTranslate(string text)
        {
            try
            {
                return ItemStats_Dictionary.ItemStatsDictionary.FirstOrDefault(s => s.Value == text && s.Key.language == Settings1.Default.statslanguage).Key.text;
            }
            catch { return ""; }
            
        }
        public string LabelTranslate(string text)
        {
            try
            {
                return Translate_Dictionary.translate.FirstOrDefault(s => s.Value == text).Key;
            }
            catch { return ""; }
        }
        public string ModsTranslate(List<string> Mods)
        {
            string t = "";
            if (Mods != null)
            {
                string statspattern = @"[+\-]?\d+(\.\d+)?";
                string pattern = @"\{0\}";
                for (int i = 0; i < Mods.Count; i++)
                {
                    string temp = StatsTranslate(Regex.Replace(Mods[i], statspattern, "#"))??"";
                    MatchCollection matches = Regex.Matches(Mods[i], statspattern);
                    int j = 0;
                    t += Regex.Replace(temp, pattern, match =>
                    {
                        return matches[j++].Value.ToString();
                    }) + "\r\n";
                }
            }
            return t;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }




    }
}
