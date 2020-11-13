#region File Description
//-----------------------------------------------------------------------------
// SettingsSerializer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
#endregion

namespace Movipa.Util
{
    /// <summary>
    /// Reads and writes parameters to XML.
    /// Values defined in this class can be used by specifying an
    /// optional type in the template. However, supported types are limited to 
    /// those which can be serialized to XML.
    /// 
    /// XML�փp�����[�^���������񂾂�A�ǂݍ��񂾂肵�܂��B
    /// �e���v���[�g�ɔC�ӂ̌^���w�肷�鎖�ɂ���āA�N���X�Őݒ肳��Ă���l��
    /// �������Ƃ��\�ł��B�������A�T�|�[�g���Ă���^��XML�ւ̃V���A���C�Y��
    /// �T�|�[�g����Ă��镨�Ɍ���܂��B
    /// </summary>
    public static class SettingsSerializer
    {
        #region Serialize Method
        /// <summary>
        /// Serializes and saves to the file.
        /// 
        /// �V���A���C�Y���ăt�@�C���ɕۑ����܂��B
        /// </summary>
        /// <param name="filename">File name</param>
        ///  
        /// <param name="filename">�t�@�C����</param>
        public static void SaveSaveData(string filename, SaveData saveData)
        {
            // Opens the file or creates a new one.
            // 
            // �t�@�C�����J���A�܂��͐V�K�쐬�����邵�܂��B
            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                // Serializes and writes the content.
                // 
                // ���e���V���A���C�Y���ď������݂܂��B
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, saveData);
                stream.Flush();
            }
        }


        /// <summary>
        /// Loads the serialized information.
        /// Returns NULL if the file cannot be found. 
        /// 
        /// �V���A���C�Y���ꂽ����ǂݍ��݂܂��B
        /// �t�@�C����������Ȃ������ꍇ��NULL���߂�܂��B
        /// </summary>
        /// <param name="filename">File name</param>
        ///  
        /// <param name="filename">�t�@�C����</param>
        public static SaveData LoadSaveData(string filename)
        {
            SaveData settings = default(SaveData);

            // Checks for file; if file cannot be found, returns the default value.
            // 
            // �t�@�C���̗L�����`�F�b�N���A������Ȃ���Ώ����l��Ԃ��܂��B
            if (File.Exists(filename) == false)
            {
                return settings;
            }

            // Opens the file as a stream.
            // 
            // �t�@�C�����X�g���[���ŊJ���܂��B
            using (FileStream stream = File.Open(filename, FileMode.OpenOrCreate))
            {
                StreamReader streamReader = new StreamReader(stream);

                // Reads the file content to the end.
                // 
                // �t�@�C���̓��e���Ō�܂œǂݍ��݂܂��B
                string xml = streamReader.ReadToEnd();

                // Restores the values.
                // 
                // �l�𕜌����܂��B
                settings = DeserializeSaveData(xml);
            }

            return settings;
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Performs deserialization from XML. 
        /// 
        /// XML����f�V���A���C�Y�����s���܂��B
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">XML text string</param>
        ///  
        /// <param name="xml">XML�̕�����</param>

        /// <returns>Restored setting values</returns>
        ///  
        /// <returns>�������ꂽ�ݒ���̒l</returns>

        private static SaveData DeserializeSaveData(string xml)
        {
            SaveData settings = default(SaveData);

            // Creates stream from text.
            // 
            // �e�L�X�g����X�g���[�����쐬���܂��B
            using (StringReader stream = new StringReader(xml))
            {
                // Restores the values.
                // 
                // �l�𕜌����܂��B
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                settings = (SaveData)serializer.Deserialize(stream);
            }

            return settings;
        }
        #endregion
    }
}