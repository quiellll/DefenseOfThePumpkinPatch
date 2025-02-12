#region Copyright
// MIT License
// 
// Copyright (c) 2023 David María Arribas
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;


public class BinaryGameDataSaver : IGameDataSaver
{
    private string _dataFolder;
    private BinaryFormatter _formatter;
        
    public void Initialize()
    {
        _dataFolder = Application.dataPath + "/Saves/Binary";
        if(!Directory.Exists(_dataFolder)) Directory.CreateDirectory(_dataFolder);
        _formatter = new();
    }
        
    public void Save<T>(string fileName, T data)
    {
        string filePath = $"{_dataFolder}/{fileName}.save";
        FileStream fileStream = File.Create(filePath);
        _formatter.Serialize(fileStream, data);
        fileStream.Flush();
        fileStream.Close();

        Debug.Log("data saved");

    }

    public bool Load<T>(string fileName, out T data)
    {
        string filePath = $"{_dataFolder}/{fileName}.save";
        if (!File.Exists(filePath))
        {
            data = default;
            return false;
        }
            
        FileStream fileStream = File.OpenRead(filePath);
        data = (T)_formatter.Deserialize(fileStream);
        fileStream.Close();

        Debug.Log("data loaded");

        return true;
    }

    public bool ExistsSave(string fileName) => File.Exists($"{_dataFolder}/{fileName}.save");

    public void Delete(string fileName)
    {
        string filePath = $"{_dataFolder}/{fileName}.save";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Save file '{fileName}.save' deleted");
        }
        else
        {
            Debug.LogWarning($"Save file '{fileName}.save' not found. Unable to delete.");
        }
    }
}
