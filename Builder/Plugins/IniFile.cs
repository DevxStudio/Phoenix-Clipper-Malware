using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// INI класс, для работы с ini-файлами [v0.2] 
/// (github.com/0xLaileb/IniFile)
/// </summary>
public class IniFile
{
    private readonly string FileName; //Имя файла инициализации.
    /// <param name="FileName">Имя файла инициализации.</param>
    public IniFile(string FileName = null) => this.FileName = new System.IO.FileInfo(FileName).FullName.ToString();

    #region KERNEL32
    /// <summary>
    /// WritePrivateProfileString устанавливает строковые значения в INI-файлах. 
    /// Также функция может быть использована для установки числовых значений, если использовать их в строковом виде (например: "1"). 
    /// Если файл, в который вы пытаетесь установить значение, не существует, он будет создан. 
    /// Аналогично, будут созданы секции, ключи и значения ключей. 
    /// Обратите внимание, что поддержка INI -файлов обеспечивается в Windows для совместимости; для хранения информации используйте системный реестр.
    /// </summary>
    /// 
    /// <param name="lpAppName">
    /// Значение секции INI-файла.
    /// </param>
    /// <param name="lpKeyName">
    /// Значение ключа.
    /// </param>
    /// <param name="lpString">
    /// Устанавлимое строковое значение.
    /// </param>
    /// <param name="lpFileName">
    /// Имя INI-файла.
    /// </param>
    /// 
    /// <returns>
    /// Функция возвращает 0 при ошибке и ненулевое значение при успешном выполнении.
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern long WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

    /// <summary>
    /// GetPrivateProfileString читает строковые значения из INI-файлов. 
    /// Параметры, передаваемые функции, определяют значение для чтения. 
    /// </summary>
    /// 
    /// <param name="lpAppName">
    /// Имя секции, содержащего имя ключа. 
    /// Если этот параметр равен нулю, то Функция GetPrivateProfileString копирует все имена секций в файле в предоставленный буфер.
    /// </param>
    /// <param name="lpKeyName">
    /// Имя ключа, связанная строка которого должна быть получена. 
    /// Если этот параметр равен нулю, то все имена ключей в секции, указанном параметром lpAppName, копируются в буфер, указанный параметром lpReturnedString.
    /// </param>
    /// <param name="lpDefault">
    /// Строка по умолчанию. Если ключ lpKeyName не может быть найден в файле инициализации, 
    /// GetPrivateProfileString копирует строку по умолчанию в буфер lpReturnedString.
    /// Если этот параметр имеет значение NULL, то по умолчанию используется пустая строка "".
    /// Избегайте указания строки по умолчанию с конечными пустыми символами.Функция вставляет нулевой символ в буфер lpReturnedString, чтобы удалить все завершающие пробелы.
    /// </param>
    /// <param name="lpReturnedString">
    /// Буфер, который получает полученную строку.
    /// </param>
    /// <param name="nSize">
    /// Размер буфера, на который указывает параметр lpReturnedString, в символах.
    /// </param>
    /// <param name="lpFileName">
    /// Имя файла инициализации. Если этот параметр не содержит полного пути к файлу, система выполняет поиск файла в каталоге Windows.
    /// </param>
    /// 
    /// <returns>
    /// Функция всегда возвращает длину в символах строки, помещенной в переменную lpReturnedString. 
    /// Если выполнение функции было успешно, чтение строки от INI файла будет помещено в lpReturnedString. 
    /// Если нет, то вместо этого получит строку, данную как lpDefault. 
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

    /// <summary>
    /// GetPrivateProfileInt читает целое значение заданного ключа из ini-файла.
    /// </summary>
    /// 
    /// <param name="lpAppName">
    /// Секция, где идет поиск значения ключа.
    /// </param>
    /// <param name="lpKeyName">
    /// Ключ, значение которого требуется узнать.
    /// </param>
    /// <param name="nDefault">
    /// Значение по умолчанию, если заданный ключ не будет найден.
    /// </param>
    /// <param name="lpFileName">
    /// Путь к ini-файлу. Если путь не указан, то файл ищется в каталоге Windows.
    /// </param>
    /// 
    /// <returns>
    /// В успешном случае функция возвращает значение ключа. 
    /// Если заданное значение не существует, или является строкой, то возвращается значение, определенное как nDefault. 
    /// Если полученное значение не является допустимым целым числом, то возвратится только числовая часть (например: 10яблок = 10).
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

    /// <summary>
    /// GetPrivateProfileSection извлекает все ключи и значения для указанной секции файла инициализации.
    /// </summary>
    /// 
    /// <param name="lpAppName">
    /// Имя секции в файле инициализации.
    /// </param>
    /// <param name="lpReturnedString">
    /// Буфер, который получает пары имен ключей и значений, связанные с именованной секцией. 
    /// Буфер заполняется одной или несколькими строками с нулевым окончанием; за последней строкой следует второй нулевой символ.
    /// </param>
    /// <param name="nSize">
    /// Размер буфера, на который указывает параметр lpReturnedString, в символах.
    /// </param>
    /// <param name="lpFileName">
    /// Имя файла инициализации.
    /// </param>
    /// 
    /// <returns>
    /// Возвращаемое значение указывает количество символов, скопированных в буфер, не включая завершающий нулевой символ. 
    /// Если буфер недостаточно велик, чтобы содержать все пары имен ключей и значений, связанные с именованной секцией, возвращаемое значение равно nSize минус два.
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern int GetPrivateProfileSection(string lpAppName, System.IntPtr lpReturnedString, int nSize, string lpFileName);

    /// <summary>
    /// GetPrivateProfileSectionNames извлекает имена всех секций в файле инициализации.
    /// </summary>
    /// 
    /// <param name="lpReturnedString">
    /// Буфер, который получает имена секций, связанных с именованным файлом. 
    /// Буфер заполняется одной или несколькими строками с нулевымокончанием; за последней строкой следует второй нулевой символ.
    /// </param>
    /// <param name="nSize">
    /// Размер буфера, на который указывает параметр lpszReturnBuffer, в символах.
    /// </param>
    /// <param name="lpFileName">
    /// Имя файла инициализации. Если этот параметр равен нулю, функция ищет выигрыш.ini-файл.
    /// </param>
    /// 
    /// <returns>
    /// Возвращаемое значение указывает количество символов, скопированных в указанный буфер, не включая завершающий нулевой символ. 
    /// Если буфер недостаточно велик, чтобы содержать все имена секций, связанные с указанным файлом инициализации, возвращаемое значение равно размеру, указанному параметром nSize минус два.
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern int GetPrivateProfileSectionNames(System.IntPtr lpReturnedString, int nSize, string lpFileName);

    #endregion

    #region ИНСТРУМЕНТЫ
    /// <summary>
    /// Write устанавливает строковые значения в ini-файлах. 
    /// Также функция может быть использована для установки числовых значений, если использовать их в строковом виде (например: "1"). 
    /// Если файл, в который вы пытаетесь установить значение, не существует, он будет создан. 
    /// Аналогично, будут созданы секции, ключи и значения ключей. 
    /// Обратите внимание, что поддержка ini-файлов обеспечивается в Windows для совместимости; для хранения информации используйте системный реестр.
    /// </summary>
    /// 
    /// <param name="Section">
    /// Значение секции ini-файла.
    /// </param>
    /// <param name="Key">
    /// Значение ключа.
    /// </param>
    /// <param name="Value">
    /// Устанавлимое строковое значение.
    /// </param>
    /// 
    /// <returns>
    /// Функция возвращает 0 при ошибке и ненулевое значение при успешном выполнении.
    /// </returns>
    public void Write(string Key, string Value, string Section = null) => WritePrivateProfileString(Section, Key, Value, FileName);

    /// <summary>
    /// ReadString читает строковые значения из ini-файлов. 
    /// Параметры, передаваемые функции, определяют значение для чтения. 
    /// </summary>
    /// 
    /// <param name="Key">
    /// Имя ключа, связанная строка которого должна быть получена. 
    /// Если этот параметр равен нулю, то все имена ключей в секции, указанном параметром Section, возвращаются.
    /// </param>
    /// <param name="Default">
    /// Если ключ Key не может быть найден в файле инициализации, то возвращается Default.
    /// </param>
    /// <param name="Section">
    /// Имя секции, содержащего имя ключа. 
    /// Если этот параметр равен нулю, то Функция ReadString возвращает все имена секций в файле.
    /// </param>
    /// 
    /// <returns>
    /// Функция всегда возвращает длину в символах строки.
    /// Если выполнение функции было успешно, чтение строки от INI файла будет возвращено. 
    /// Если нет, то вместо этого получит строку, данную как Default. 
    /// </returns>
    public string ReadString(string Key, string Section = null, int Size = 99999999, string Default = "")
    {
        StringBuilder tmp = new StringBuilder(Size); //Создаем буфер, который будет получать значение.
        GetPrivateProfileString(Section, Key, Default, tmp, Size, FileName);
        return tmp.ToString();
    }

    /// <summary>
    /// ReadInt читает числовое значение заданного ключа из ini-файла.
    /// </summary>
    /// 
    /// <param name="Section">
    /// Секция, где идет поиск значения ключа.
    /// </param>
    /// <param name="Key">
    /// Ключ, значение которого требуется узнать.
    /// </param>
    /// <param name="Default">
    /// Значение по умолчанию, если заданный ключ не будет найден.
    /// </param>
    /// 
    /// <returns>
    /// В успешном случае функция возвращает значение ключа. 
    /// Если заданное значение не существует, или является строкой, то возвращается значение, определенное как Default. 
    /// Если полученное значение не является допустимым целым числом, то возвратится только числовая часть (например: 10яблок = 10).
    /// </returns>
    public int ReadInt(string Key, string Section = null, int Default = -1) => GetPrivateProfileInt(Section, Key, Default, FileName);

    /// <summary>
    /// ReadBool читает логическое значение заданного ключа из ini-файла.
    /// </summary>
    /// 
    /// <param name="Key">
    /// Ключ, значение которого требуется узнать.
    /// </param>
    /// <param name="Section">
    /// Секция, где идет поиск значения ключа.
    /// </param>
    /// 
    /// <returns>
    /// В успешном случае функция возвращает значение ключа (true или false), если оно было найдено.
    /// </returns>
    public bool ReadBool(string Key, string Section = null, int Size = 255)
    {
        StringBuilder tmp = new StringBuilder(Size); //Создаем буфер, который будет получать значение.
        GetPrivateProfileString(Section, Key, "", tmp, Size, FileName);
        return System.Convert.ToBoolean(tmp.ToString());
    }

    /// <summary>
    /// GetAllDataSection извлекает все ключи и значения для указанной секции файла инициализации.
    /// </summary>
    /// 
    /// <param name="Section">
    /// Имя секции в файле инициализации.
    /// </param>
    /// 
    /// <returns>
    /// Возвращаемое значение указывает количество символов.
    /// </returns>
    public string[] GetAllDataSection(string Section, int Size = 255)
    {
        System.IntPtr pMem = Marshal.AllocHGlobal(4096 * sizeof(char));
        string temp = string.Empty;

        int count = GetPrivateProfileSection(Section, pMem, Size * sizeof(char), FileName) - 1;
        if (count > 0) temp = Marshal.PtrToStringUni(pMem, count);
        Marshal.FreeHGlobal(pMem);

        return temp.Split('\0');
    }

    /// <summary>
    /// GetAllSections извлекает имена всех секций в файле инициализации.
    /// </summary>
    /// 
    /// <returns>
    /// Возвращаемое значение указывает количество символов.
    /// </returns>
    public string[] GetAllSections(int Size = 255)
    {
        System.IntPtr pMem = Marshal.AllocHGlobal(4096 * sizeof(char));
        string temp = string.Empty;

        int count = GetPrivateProfileSectionNames(pMem, Size * sizeof(char), FileName) - 1;
        if (count > 0) temp = Marshal.PtrToStringUni(pMem, count);
        Marshal.FreeHGlobal(pMem);

        return temp.Split('\0');
    }

    /// <summary>
    /// DeleteKey удаляет значение заданного ключа в определенной секции.
    /// </summary>
    /// 
    /// <param name="Key">
    /// Имя ключа, значение которого должно быть удалено.
    /// </param>
    /// <param name="Section">
    /// Имя секции, содержащего имя ключа.
    /// </param>
    public void DeleteKey(string Key, string Section = null) => Write(Key, null, Section);

    /// <summary>
    /// DeleteSection удаляет заданную секцию.
    /// </summary>
    /// 
    /// <param name="Section">
    /// Имя секции, который нужно удалить.
    /// </param>
    public void DeleteSection(string Section = null) => Write(null, null, Section);

    /// <summary>
    /// KeyExists производит чтение ключа по определенной секции и проверяет наличие значения.
    /// </summary>
    /// 
    /// <param name="Key">
    /// Имя ключа, значение которого будет проверяться (value.Length > 0)
    /// </param>
    /// <param name="Section">
    /// Имя секции, в котором находится Key.
    /// </param>
    /// 
    /// <returns>
    /// Возвращает true, если ключ имеет значение, иначе возвращает false.
    /// </returns>
    public bool KeyExists(string Key, string Section = null) => ReadString(Key, Section).Length > 0;
    #endregion
}
