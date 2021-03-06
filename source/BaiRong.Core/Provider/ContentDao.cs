﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.Model;
using BaiRong.Core.Model.Enumerations;
using SiteServer.Plugin.Models;

namespace BaiRong.Core.Provider
{
    public class ContentDao : DataProviderBase
    {
        public const int TaxisMaxValue = 2147483647;
        public const int TaxisIsTopStartValue = 2147480000;

        public int Insert(string tableName, IContentInfo contentInfo)
        {
            //var contentId = 0;

            if (string.IsNullOrEmpty(tableName) || contentInfo == null) return 0;

            contentInfo.LastEditDate = DateTime.Now;

            var metadataInfoList = TableManager.GetTableMetadataInfoList(tableName);

            var names = new StringBuilder();
            var values = new StringBuilder();
            var paras = new List<IDataParameter>();
            var lowerCaseExcludeAttributesNames = new List<string>(ContentAttribute.AllAttributes);
            foreach (var metadataInfo in metadataInfoList)
            {
                lowerCaseExcludeAttributesNames.Add(metadataInfo.AttributeName.ToLower());
                names.Append($",{metadataInfo.AttributeName}").AppendLine();
                values.Append($",@{metadataInfo.AttributeName}").AppendLine();
                if (metadataInfo.DataType == DataType.Integer)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetInt(metadataInfo.AttributeName)));
                }
                else if (metadataInfo.DataType == DataType.Decimal)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetDecimal(metadataInfo.AttributeName)));
                }
                else if (metadataInfo.DataType == DataType.Boolean)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetBool(metadataInfo.AttributeName)));
                }
                else if (metadataInfo.DataType == DataType.DateTime)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetDateTime(metadataInfo.AttributeName, DateTime.Now)));
                }
                else
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetExtendedAttribute(metadataInfo.AttributeName)));
                }
            }

            var sqlString = $@"
INSERT INTO {tableName} (
    {nameof(ContentInfo.NodeId)},
    {nameof(ContentInfo.PublishmentSystemId)},
    {nameof(ContentInfo.AddUserName)},
    {nameof(ContentInfo.LastEditUserName)},
    {nameof(ContentInfo.WritingUserName)},
    {nameof(ContentInfo.LastEditDate)},
    {nameof(ContentInfo.Taxis)},
    {nameof(ContentInfo.ContentGroupNameCollection)},
    {nameof(ContentInfo.Tags)},
    {nameof(ContentInfo.SourceId)},
    {nameof(ContentInfo.ReferenceId)},
    {nameof(ContentInfo.IsChecked)},
    {nameof(ContentInfo.CheckedLevel)},
    {nameof(ContentInfo.Comments)},
    {nameof(ContentInfo.Photos)},
    {nameof(ContentInfo.Hits)},
    {nameof(ContentInfo.HitsByDay)},
    {nameof(ContentInfo.HitsByWeek)},
    {nameof(ContentInfo.HitsByMonth)},
    {nameof(ContentInfo.LastHitsDate)},
    {nameof(ContentInfo.SettingsXml)},
    {nameof(ContentInfo.Title)},
    {nameof(ContentInfo.IsTop)},
    {nameof(ContentInfo.IsRecommend)},
    {nameof(ContentInfo.IsHot)},
    {nameof(ContentInfo.IsColor)},
    {nameof(ContentInfo.AddDate)}
    {names}
) VALUES (
    @{nameof(ContentInfo.NodeId)},
    @{nameof(ContentInfo.PublishmentSystemId)},
    @{nameof(ContentInfo.AddUserName)},
    @{nameof(ContentInfo.LastEditUserName)},
    @{nameof(ContentInfo.WritingUserName)},
    @{nameof(ContentInfo.LastEditDate)},
    @{nameof(ContentInfo.Taxis)},
    @{nameof(ContentInfo.ContentGroupNameCollection)},
    @{nameof(ContentInfo.Tags)},
    @{nameof(ContentInfo.SourceId)},
    @{nameof(ContentInfo.ReferenceId)},
    @{nameof(ContentInfo.IsChecked)},
    @{nameof(ContentInfo.CheckedLevel)},
    @{nameof(ContentInfo.Comments)},
    @{nameof(ContentInfo.Photos)},
    @{nameof(ContentInfo.Hits)},
    @{nameof(ContentInfo.HitsByDay)},
    @{nameof(ContentInfo.HitsByWeek)},
    @{nameof(ContentInfo.HitsByMonth)},
    @{nameof(ContentInfo.LastHitsDate)},
    @{nameof(ContentInfo.SettingsXml)},
    @{nameof(ContentInfo.Title)},
    @{nameof(ContentInfo.IsTop)},
    @{nameof(ContentInfo.IsRecommend)},
    @{nameof(ContentInfo.IsHot)},
    @{nameof(ContentInfo.IsColor)},
    @{nameof(ContentInfo.AddDate)}
    {values}
)";

            var parameters = new List<IDataParameter>
            {
                GetParameter($"@{nameof(ContentInfo.NodeId)}", DataType.Integer, contentInfo.NodeId),
                GetParameter($"@{nameof(ContentInfo.PublishmentSystemId)}", DataType.Integer, contentInfo.PublishmentSystemId),
                GetParameter($"@{nameof(ContentInfo.AddUserName)}", DataType.VarChar, 255, contentInfo.AddUserName),
                GetParameter($"@{nameof(ContentInfo.LastEditUserName)}", DataType.VarChar, 255, contentInfo.LastEditUserName),
                GetParameter($"@{nameof(ContentInfo.WritingUserName)}", DataType.VarChar, 255, contentInfo.WritingUserName),
                GetParameter($"@{nameof(ContentInfo.LastEditDate)}", DataType.DateTime, contentInfo.LastEditDate),
                GetParameter($"@{nameof(ContentInfo.Taxis)}", DataType.Integer, contentInfo.Taxis),
                GetParameter($"@{nameof(ContentInfo.ContentGroupNameCollection)}", DataType.VarChar, 255, contentInfo.ContentGroupNameCollection),
                GetParameter($"@{nameof(ContentInfo.Tags)}", DataType.VarChar, 255, contentInfo.Tags),
                GetParameter($"@{nameof(ContentInfo.SourceId)}", DataType.Integer, contentInfo.SourceId),
                GetParameter($"@{nameof(ContentInfo.ReferenceId)}", DataType.Integer, contentInfo.ReferenceId),
                GetParameter($"@{nameof(ContentInfo.IsChecked)}", DataType.VarChar, 18, contentInfo.IsChecked.ToString()),
                GetParameter($"@{nameof(ContentInfo.CheckedLevel)}", DataType.Integer, contentInfo.CheckedLevel),
                GetParameter($"@{nameof(ContentInfo.Comments)}", DataType.Integer, contentInfo.Comments),
                GetParameter($"@{nameof(ContentInfo.Photos)}", DataType.Integer, contentInfo.Photos),
                GetParameter($"@{nameof(ContentInfo.Hits)}", DataType.Integer, contentInfo.Hits),
                GetParameter($"@{nameof(ContentInfo.HitsByDay)}", DataType.Integer, contentInfo.HitsByDay),
                GetParameter($"@{nameof(ContentInfo.HitsByWeek)}", DataType.Integer, contentInfo.HitsByWeek),
                GetParameter($"@{nameof(ContentInfo.HitsByMonth)}", DataType.Integer, contentInfo.HitsByMonth),
                GetParameter($"@{nameof(ContentInfo.LastHitsDate)}", DataType.DateTime, contentInfo.LastHitsDate),
                GetParameter($"@{nameof(ContentInfo.SettingsXml)}", DataType.Text, contentInfo.Attributes.ToString(lowerCaseExcludeAttributesNames)),
                GetParameter($"@{nameof(ContentInfo.Title)}", DataType.VarChar, 255, contentInfo.Title),
                GetParameter($"@{nameof(ContentInfo.IsTop)}", DataType.VarChar, 18, contentInfo.IsTop.ToString()),
                GetParameter($"@{nameof(ContentInfo.IsRecommend)}", DataType.VarChar, 18, contentInfo.IsRecommend.ToString()),
                GetParameter($"@{nameof(ContentInfo.IsHot)}", DataType.VarChar, 18, contentInfo.IsHot.ToString()),
                GetParameter($"@{nameof(ContentInfo.IsColor)}", DataType.VarChar, 18, contentInfo.IsColor.ToString()),
                GetParameter($"@{nameof(ContentInfo.AddDate)}", DataType.DateTime, contentInfo.AddDate)
            };
            parameters.AddRange(paras);

            //IDataParameter[] parms;
            //var sqlInsert = BaiRongDataProvider.DatabaseDao.GetInsertSqlString(contentInfo.Attributes.GetExtendedAttributes(), tableName, out parms);

            return ExecuteNonQueryAndReturnId(tableName, nameof(ContentInfo.Id), sqlString, parameters.ToArray());

            //using (var conn = GetConnection())
            //{
            //    conn.Open();
            //    using (var trans = conn.BeginTransaction())
            //    {
            //        try
            //        {
            //            //contentId = ExecuteNonQueryAndReturnId(trans, sqlInsert, parms);
            //            contentId = ExecuteNonQueryAndReturningId(trans, sqlString, nameof(ContentInfo.Id), parameters.ToArray());

            //            trans.Commit();
            //        }
            //        catch
            //        {
            //            trans.Rollback();
            //            throw;
            //        }
            //    }
            //}

            //return contentId;
        }

        public void Update(string tableName, IContentInfo contentInfo)
        {
            if (string.IsNullOrEmpty(tableName) || contentInfo == null) return;

            //出现IsTop与Taxis不同步情况
            if (contentInfo.IsTop == false && contentInfo.Taxis >= TaxisIsTopStartValue)
            {
                contentInfo.Taxis = BaiRongDataProvider.ContentDao.GetMaxTaxis(tableName, contentInfo.NodeId, false) + 1;
            }
            else if (contentInfo.IsTop && contentInfo.Taxis < TaxisIsTopStartValue)
            {
                contentInfo.Taxis = BaiRongDataProvider.ContentDao.GetMaxTaxis(tableName, contentInfo.NodeId, true) + 1;
            }

            contentInfo.LastEditDate = DateTime.Now;

            //if (!string.IsNullOrEmpty(tableName))
            //{
            //    contentInfo.Attributes.BeforeExecuteNonQuery();
            //    sqlString = BaiRongDataProvider.DatabaseDao.GetUpdateSqlString(contentInfo.Attributes.GetExtendedAttributes(), tableName, out parms);
            //}

            var metadataInfoList = TableManager.GetTableMetadataInfoList(tableName);

            var sets = new StringBuilder();
            var paras = new List<IDataParameter>();
            var lowerCaseExcludeAttributesNames = new List<string>(ContentAttribute.AllAttributes);
            foreach (var metadataInfo in metadataInfoList)
            {
                lowerCaseExcludeAttributesNames.Add(metadataInfo.AttributeName.ToLower());
                sets.Append($",{metadataInfo.AttributeName} = @{metadataInfo.AttributeName}").AppendLine();
                if (metadataInfo.DataType == DataType.Integer)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetInt(metadataInfo.AttributeName)));
                }
                else if (metadataInfo.DataType == DataType.Decimal)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetDecimal(metadataInfo.AttributeName)));
                }
                else if (metadataInfo.DataType == DataType.Boolean)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetBool(metadataInfo.AttributeName)));
                }
                else if (metadataInfo.DataType == DataType.DateTime)
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetDateTime(metadataInfo.AttributeName, DateTime.Now)));
                }
                else
                {
                    paras.Add(GetParameter($"@{metadataInfo.AttributeName}", metadataInfo.DataType, contentInfo.Attributes.GetExtendedAttribute(metadataInfo.AttributeName)));
                }
            }

            var sqlString = $@"
UPDATE {tableName} SET 
    {nameof(ContentInfo.NodeId)} = @{nameof(ContentInfo.NodeId)},
    {nameof(ContentInfo.PublishmentSystemId)} = @{nameof(ContentInfo.PublishmentSystemId)},
    {nameof(ContentInfo.AddUserName)} = @{nameof(ContentInfo.AddUserName)},
    {nameof(ContentInfo.LastEditUserName)} = @{nameof(ContentInfo.LastEditUserName)},
    {nameof(ContentInfo.WritingUserName)} = @{nameof(ContentInfo.WritingUserName)},
    {nameof(ContentInfo.LastEditDate)} = @{nameof(ContentInfo.LastEditDate)},
    {nameof(ContentInfo.Taxis)} = @{nameof(ContentInfo.Taxis)},
    {nameof(ContentInfo.ContentGroupNameCollection)} = @{nameof(ContentInfo.ContentGroupNameCollection)},
    {nameof(ContentInfo.Tags)} = @{nameof(ContentInfo.Tags)},
    {nameof(ContentInfo.SourceId)} = @{nameof(ContentInfo.SourceId)},
    {nameof(ContentInfo.ReferenceId)} = @{nameof(ContentInfo.ReferenceId)},
    {nameof(ContentInfo.IsChecked)} = @{nameof(ContentInfo.IsChecked)},
    {nameof(ContentInfo.CheckedLevel)} = @{nameof(ContentInfo.CheckedLevel)},
    {nameof(ContentInfo.Comments)} = @{nameof(ContentInfo.Comments)},
    {nameof(ContentInfo.Photos)} = @{nameof(ContentInfo.Photos)},
    {nameof(ContentInfo.Hits)} = @{nameof(ContentInfo.Hits)},
    {nameof(ContentInfo.HitsByDay)} = @{nameof(ContentInfo.HitsByDay)},
    {nameof(ContentInfo.HitsByWeek)} = @{nameof(ContentInfo.HitsByWeek)},
    {nameof(ContentInfo.HitsByMonth)} = @{nameof(ContentInfo.HitsByMonth)},
    {nameof(ContentInfo.LastHitsDate)} = @{nameof(ContentInfo.LastHitsDate)},
    {nameof(ContentInfo.SettingsXml)} = @{nameof(ContentInfo.SettingsXml)},
    {nameof(ContentInfo.Title)} = @{nameof(ContentInfo.Title)},
    {nameof(ContentInfo.IsTop)} = @{nameof(ContentInfo.IsTop)},
    {nameof(ContentInfo.IsRecommend)} = @{nameof(ContentInfo.IsRecommend)},
    {nameof(ContentInfo.IsHot)} = @{nameof(ContentInfo.IsHot)},
    {nameof(ContentInfo.IsColor)} = @{nameof(ContentInfo.IsColor)},
    {nameof(ContentInfo.AddDate)} = @{nameof(ContentInfo.AddDate)}
    {sets}
WHERE {nameof(ContentInfo.Id)} = @{nameof(ContentInfo.Id)}";

            var parameters = new List<IDataParameter>
            {
                GetParameter($"@{nameof(ContentInfo.NodeId)}", DataType.Integer, contentInfo.NodeId),
                GetParameter($"@{nameof(ContentInfo.PublishmentSystemId)}", DataType.Integer, contentInfo.PublishmentSystemId),
                GetParameter($"@{nameof(ContentInfo.AddUserName)}", DataType.VarChar, 255, contentInfo.AddUserName),
                GetParameter($"@{nameof(ContentInfo.LastEditUserName)}", DataType.VarChar, 255, contentInfo.LastEditUserName),
                GetParameter($"@{nameof(ContentInfo.WritingUserName)}", DataType.VarChar, 255, contentInfo.WritingUserName),
                GetParameter($"@{nameof(ContentInfo.LastEditDate)}", DataType.DateTime, contentInfo.LastEditDate),
                GetParameter($"@{nameof(ContentInfo.Taxis)}", DataType.Integer, contentInfo.Taxis),
                GetParameter($"@{nameof(ContentInfo.ContentGroupNameCollection)}", DataType.VarChar, 255, contentInfo.ContentGroupNameCollection),
                GetParameter($"@{nameof(ContentInfo.Tags)}", DataType.VarChar, 255, contentInfo.Tags),
                GetParameter($"@{nameof(ContentInfo.SourceId)}", DataType.Integer, contentInfo.SourceId),
                GetParameter($"@{nameof(ContentInfo.ReferenceId)}", DataType.Integer, contentInfo.ReferenceId),
                GetParameter($"@{nameof(ContentInfo.IsChecked)}", DataType.VarChar, 18, contentInfo.IsChecked.ToString()),
                GetParameter($"@{nameof(ContentInfo.CheckedLevel)}", DataType.Integer, contentInfo.CheckedLevel),
                GetParameter($"@{nameof(ContentInfo.Comments)}", DataType.Integer, contentInfo.Comments),
                GetParameter($"@{nameof(ContentInfo.Photos)}", DataType.Integer, contentInfo.Photos),
                GetParameter($"@{nameof(ContentInfo.Hits)}", DataType.Integer, contentInfo.Hits),
                GetParameter($"@{nameof(ContentInfo.HitsByDay)}", DataType.Integer, contentInfo.HitsByDay),
                GetParameter($"@{nameof(ContentInfo.HitsByWeek)}", DataType.Integer, contentInfo.HitsByWeek),
                GetParameter($"@{nameof(ContentInfo.HitsByMonth)}", DataType.Integer, contentInfo.HitsByMonth),
                GetParameter($"@{nameof(ContentInfo.LastHitsDate)}", DataType.DateTime, contentInfo.LastHitsDate),
                GetParameter($"@{nameof(ContentInfo.SettingsXml)}", DataType.Text, contentInfo.Attributes.ToString(lowerCaseExcludeAttributesNames)),
                GetParameter($"@{nameof(ContentInfo.Title)}", DataType.VarChar, 255, contentInfo.Title),
                GetParameter($"@{nameof(ContentInfo.IsTop)}", DataType.VarChar, 18, contentInfo.IsTop.ToString()),
                GetParameter($"@{nameof(ContentInfo.IsRecommend)}", DataType.VarChar, 18, contentInfo.IsRecommend.ToString()),
                GetParameter($"@{nameof(ContentInfo.IsHot)}", DataType.VarChar, 18, contentInfo.IsHot.ToString()),
                GetParameter($"@{nameof(ContentInfo.IsColor)}", DataType.VarChar, 18, contentInfo.IsColor.ToString()),
                GetParameter($"@{nameof(ContentInfo.AddDate)}", DataType.DateTime, contentInfo.AddDate)
            };
            parameters.AddRange(paras);
            parameters.Add(GetParameter($"@{nameof(ContentInfo.Id)}", DataType.Integer, contentInfo.Id));

            ExecuteNonQuery(sqlString, parameters.ToArray());
        }

        public bool UpdateTaxisToUp(string tableName, int nodeId, int contentId, bool isTop)
        {
            //Get Higher Taxis and Id
            var sqlString = SqlUtils.GetTopSqlString(tableName, "Id, Taxis",
                isTop
                    ? $"WHERE (Taxis > (SELECT Taxis FROM {tableName} WHERE Id = {contentId}) AND Taxis >= {TaxisIsTopStartValue} AND NodeId = {nodeId})"
                    : $"WHERE (Taxis > (SELECT Taxis FROM {tableName} WHERE Id = {contentId}) AND Taxis < {TaxisIsTopStartValue} AND NodeId = {nodeId})",
                "ORDER BY Taxis", 1);
            var higherId = 0;
            var higherTaxis = 0;

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherId = GetInt(rdr, 0);
                    higherTaxis = GetInt(rdr, 1);
                }
                rdr.Close();
            }

            if (higherId != 0)
            {
                //Get Taxis Of Selected Id
                var selectedTaxis = GetTaxis(contentId, tableName);

                //Set The Selected Class Taxis To Higher Level
                SetTaxis(contentId, higherTaxis, tableName);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(higherId, selectedTaxis, tableName);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(string tableName, int nodeId, int contentId, bool isTop)
        {
            //Get Lower Taxis and Id
            var sqlString = SqlUtils.GetTopSqlString(tableName, "Id, Taxis",
                isTop
                    ? $"WHERE (Taxis < (SELECT Taxis FROM {tableName} WHERE Id = {contentId}) AND Taxis >= {TaxisIsTopStartValue} AND NodeId = {nodeId})"
                    : $"WHERE (Taxis < (SELECT Taxis FROM {tableName} WHERE Id = {contentId}) AND Taxis < {TaxisIsTopStartValue} AND NodeId = {nodeId})",
                "ORDER BY Taxis DESC", 1);
            var lowerId = 0;
            var lowerTaxis = 0;

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerId = GetInt(rdr, 0);
                    lowerTaxis = GetInt(rdr, 1);
                }
                rdr.Close();
            }

            if (lowerId != 0)
            {
                //Get Taxis Of Selected Class
                var selectedTaxis = GetTaxis(contentId, tableName);

                //Set The Selected Class Taxis To Lower Level
                SetTaxis(contentId, lowerTaxis, tableName);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(lowerId, selectedTaxis, tableName);
                return true;
            }
            return false;
        }

        public int GetMaxTaxis(string tableName, int nodeId, bool isTop)
        {
            var maxTaxis = 0;
            if (isTop)
            {
                maxTaxis = TaxisIsTopStartValue;

                string sqlString =
                    $"SELECT MAX(Taxis) FROM {tableName} WHERE NodeId = {nodeId} AND Taxis >= {TaxisIsTopStartValue}";

                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var rdr = ExecuteReader(conn, sqlString))
                    {
                        if (rdr.Read())
                        {
                            maxTaxis = GetInt(rdr, 0);
                        }
                        rdr.Close();
                    }
                }
                if (maxTaxis == TaxisMaxValue)
                {
                    maxTaxis = TaxisMaxValue - 1;
                }
            }
            else
            {
                string sqlString =
                    $"SELECT MAX(Taxis) FROM {tableName} WHERE NodeId = {nodeId} AND Taxis < {TaxisIsTopStartValue}";
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var rdr = ExecuteReader(conn, sqlString))
                    {
                        if (rdr.Read())
                        {
                            maxTaxis = GetInt(rdr, 0);
                        }
                        rdr.Close();
                    }
                }
            }
            return maxTaxis;
        }

        public int GetTaxis(int selectedId, string tableName)
        {
            string sqlString = $"SELECT Taxis FROM {tableName} WHERE (Id = {selectedId})";

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        public void SetTaxis(int id, int taxis, string tableName)
        {
            string sqlString = $"UPDATE {tableName} SET Taxis = {taxis} WHERE Id = {id}";
            ExecuteNonQuery(sqlString);
        }

        public void UpdateIsChecked(string tableName, int publishmentSystemId, int nodeId, List<int> contentIdList, int translateNodeId, bool isAdmin, string userName, bool isChecked, int checkedLevel, string reasons)
        {
            if (isChecked)
            {
                checkedLevel = 0;
            }

            var checkDate = DateTime.Now;

            foreach (var contentId in contentIdList)
            {
                var settingsXml = BaiRongDataProvider.ContentDao.GetValue(tableName, contentId, ContentAttribute.SettingsXml);
                var attributes = TranslateUtils.ToNameValueCollection(settingsXml);
                attributes[ContentAttribute.CheckIsAdmin] = isAdmin.ToString();
                attributes[ContentAttribute.CheckUserName] = userName;
                attributes[ContentAttribute.CheckCheckDate] = DateUtils.GetDateAndTimeString(checkDate);
                attributes[ContentAttribute.CheckReasons] = reasons;

                string sqlString =
                    $"UPDATE {tableName} SET IsChecked = '{isChecked}', CheckedLevel = {checkedLevel}, SettingsXML = '{TranslateUtils.NameValueCollectionToString(attributes)}' WHERE Id = {contentId}";
                if (translateNodeId > 0)
                {
                    sqlString =
                        $"UPDATE {tableName} SET IsChecked = '{isChecked}', CheckedLevel = {checkedLevel}, SettingsXML = '{TranslateUtils.NameValueCollectionToString(attributes)}', NodeId = {translateNodeId} WHERE Id = {contentId}";
                }
                ExecuteNonQuery(sqlString);

                var checkInfo = new ContentCheckInfo(0, tableName, publishmentSystemId, nodeId, contentId, isAdmin, userName, isChecked, checkedLevel, checkDate, reasons);
                BaiRongDataProvider.ContentCheckDao.Insert(checkInfo);
            }
        }

        public void UpdateIsChecked(string tableName, int publishmentSystemId, int nodeId, List<int> contentIdList, int translateNodeId, bool isAdmin, string userName, bool isChecked, int checkedLevel, string reasons, bool isCheck)
        {
            if (isChecked)
            {
                checkedLevel = 0;
            }

            var checkDate = DateTime.Now;

            foreach (var contentId in contentIdList)
            {
                var settingsXml = BaiRongDataProvider.ContentDao.GetValue(tableName, contentId, ContentAttribute.SettingsXml);
                var attributes = TranslateUtils.ToNameValueCollection(settingsXml);
                attributes[ContentAttribute.CheckIsAdmin] = isAdmin.ToString();
                attributes[ContentAttribute.CheckUserName] = userName;
                attributes[ContentAttribute.CheckCheckDate] = DateUtils.GetDateAndTimeString(checkDate);
                attributes[ContentAttribute.CheckReasons] = reasons;

                string sqlString =
                    $"UPDATE {tableName} SET IsChecked = '{isChecked}', CheckedLevel = {checkedLevel}, SettingsXML = '{TranslateUtils.NameValueCollectionToString(attributes)}' WHERE Id = {contentId}";
                if (translateNodeId > 0)
                {
                    sqlString =
                        $"UPDATE {tableName} SET IsChecked = '{isChecked}', CheckedLevel = {checkedLevel}, SettingsXML = '{TranslateUtils.NameValueCollectionToString(attributes)}', NodeId = {translateNodeId} WHERE Id = {contentId}";
                }
                ExecuteNonQuery(sqlString);

                var checkInfo = new ContentCheckInfo(0, tableName, publishmentSystemId, nodeId, contentId, isAdmin, userName, isChecked, checkedLevel, checkDate, reasons);
                BaiRongDataProvider.ContentCheckDao.Insert(checkInfo);
            }
        }

        public void AddHits(string tableName, bool isCountHits, bool isCountHitsByDay, int contentId)
        {
            if (contentId > 0)
            {
                if (isCountHits)
                {
                    if (isCountHitsByDay)
                    {
                        var referenceId = 0;
                        var hitsByDay = 0;
                        var hitsByWeek = 0;
                        var hitsByMonth = 0;
                        var lastHitsDate = DateTime.Now;

                        string sqlString =
                            $"SELECT ReferenceId, HitsByDay, HitsByWeek, HitsByMonth, LastHitsDate FROM {tableName} WHERE (Id = {contentId})";

                        using (var rdr = ExecuteReader(sqlString))
                        {
                            if (rdr.Read())
                            {
                                var i = 0;
                                referenceId = GetInt(rdr, i++);
                                hitsByDay = GetInt(rdr, i++);
                                hitsByWeek = GetInt(rdr, i++);
                                hitsByMonth = GetInt(rdr, i++);
                                lastHitsDate = GetDateTime(rdr, i);
                            }
                            rdr.Close();
                        }

                        if (referenceId > 0)
                        {
                            contentId = referenceId;
                        }

                        var now = DateTime.Now;

                        hitsByDay = now.Day != lastHitsDate.Day || now.Month != lastHitsDate.Month || now.Year != lastHitsDate.Year ? 1 : hitsByDay + 1;
                        hitsByWeek = now.Month != lastHitsDate.Month || now.Year != lastHitsDate.Year || now.DayOfYear / 7 != lastHitsDate.DayOfYear / 7 ? 1 : hitsByWeek + 1;
                        hitsByMonth = now.Month != lastHitsDate.Month || now.Year != lastHitsDate.Year ? 1 : hitsByMonth + 1;

                        sqlString =
                            $"UPDATE {tableName} SET {SqlUtils.GetAddOne("Hits")}, HitsByDay = {hitsByDay}, HitsByWeek = {hitsByWeek}, HitsByMonth = {hitsByMonth}, LastHitsDate = '{DateUtils.GetDateAndTimeString(DateTime.Now)}' WHERE Id = {contentId}  AND ReferenceId = 0";
                        ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString =
                            $"UPDATE {tableName} SET {SqlUtils.GetAddOne("Hits")}, LastHitsDate = '{DateUtils.GetDateAndTimeString(DateTime.Now)}' WHERE Id = {contentId} AND ReferenceId = 0";
                        var count = ExecuteNonQuery(sqlString);
                        if (count < 1)
                        {
                            var referenceId = 0;

                            sqlString = $"SELECT ReferenceId FROM {tableName} WHERE (Id = {contentId})";

                            using (var rdr = ExecuteReader(sqlString))
                            {
                                if (rdr.Read())
                                {
                                    referenceId = GetInt(rdr, 0);
                                }
                                rdr.Close();
                            }

                            if (referenceId > 0)
                            {
                                sqlString =
                                    $"UPDATE {tableName} SET {SqlUtils.GetAddOne("Hits")}, LastHitsDate = '{DateUtils.GetDateAndTimeString(DateTime.Now)}' WHERE Id = {referenceId} AND ReferenceId = 0";
                                ExecuteNonQuery(sqlString);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateComments(string tableName, int contentId, int comments)
        {
            string sqlString = $"UPDATE {tableName} SET Comments = {comments} WHERE Id = {contentId}";
            ExecuteNonQuery(sqlString);
        }

        public void UpdatePhotos(string tableName, int contentId, int photos)
        {
            string sqlString = $"UPDATE {tableName} SET Photos = {photos} WHERE Id = {contentId}";
            ExecuteNonQuery(sqlString);
        }

        public int GetReferenceId(ETableStyle tableStyle, string tableName, int contentId, out string linkUrl, out int nodeId)
        {
            var referenceId = 0;
            nodeId = 0;
            linkUrl = string.Empty;
            try
            {
                string sqlString = $"SELECT ReferenceId, NodeId, LinkUrl FROM {tableName} WHERE Id = {contentId}";

                using (var rdr = ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        referenceId = GetInt(rdr, 0);
                        nodeId = GetInt(rdr, 1);
                        linkUrl = GetString(rdr, 2);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                // ignored
            }
            return referenceId;
        }

        public int GetReferenceId(ETableStyle tableStyle, string tableName, int contentId, out string linkUrl)
        {
            var referenceId = 0;
            linkUrl = string.Empty;
            try
            {
                string sqlString = $"SELECT ReferenceId, LinkUrl FROM {tableName} WHERE Id = {contentId}";

                using (var rdr = ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        referenceId = GetInt(rdr, 0);
                        linkUrl = GetString(rdr, 1);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                // ignored
            }
            return referenceId;
        }

        public int GetCountOfContentAdd(string tableName, int publishmentSystemId, List<int> nodeIdList, DateTime begin, DateTime end, string userName)
        {
            string sqlString;
            if (string.IsNullOrEmpty(userName))
            {
                sqlString = nodeIdList.Count == 1
                    ? $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeIdList[0]} AND (AddDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}')"
                    : $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND (AddDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}')";
            }
            else
            {
                sqlString = nodeIdList.Count == 1
                    ? $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeIdList[0]} AND (AddDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}') AND (AddUserName = '{userName}')"
                    : $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND (AddDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}') AND (AddUserName = '{userName}')";
            }

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        public int GetCountOfContentUpdate(string tableName, int publishmentSystemId, List<int> nodeIdList, DateTime begin, DateTime end, string userName)
        {
            string sqlString;
            if (string.IsNullOrEmpty(userName))
            {
                sqlString = nodeIdList.Count == 1
                    ? $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeIdList[0]} AND (LastEditDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}') AND (LastEditDate <> AddDate)"
                    : $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND (LastEditDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}') AND (LastEditDate <> AddDate)";
            }
            else
            {
                sqlString = nodeIdList.Count == 1
                    ? $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeIdList[0]} AND (LastEditDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}') AND (LastEditDate <> AddDate) AND (AddUserName = '{userName}')"
                    : $"SELECT COUNT(Id) AS Num FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND (LastEditDate BETWEEN '{begin.ToShortDateString()}' AND '{end.AddDays(1).ToShortDateString()}') AND (LastEditDate <> AddDate) AND (AddUserName = '{userName}')";
            }

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        public string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemId, List<int> nodeIdList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent)
        {
            return GetSelectCommendByCondition(tableStyle, tableName, publishmentSystemId, nodeIdList, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent, false, string.Empty);
        }

        public string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemId, List<int> nodeIdList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isWritingOnly, string userNameOnly)
        {
            if (nodeIdList == null || nodeIdList.Count == 0)
            {
                return null;
            }

            var orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            var dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = $" AND AddDate >= '{dateFrom}' ";
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += $" AND AddDate <= '{dateTo}' ";
            }
            var whereString = new StringBuilder("WHERE ");

            if (isTrashContent)
            {
                for (var i = 0; i < nodeIdList.Count; i++)
                {
                    var theNodeId = nodeIdList[i];
                    nodeIdList[i] = -theNodeId;
                }
            }

            whereString.Append(nodeIdList.Count == 1
                ? $"PublishmentSystemId = {publishmentSystemId} AND (NodeId = {nodeIdList[0]}) "
                : $"PublishmentSystemId = {publishmentSystemId} AND (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)})) ");

            if (StringUtils.EqualsIgnoreCase(searchType, ContentAttribute.IsTop) || StringUtils.EqualsIgnoreCase(searchType, ContentAttribute.IsRecommend) || StringUtils.EqualsIgnoreCase(searchType, ContentAttribute.IsColor) || StringUtils.EqualsIgnoreCase(searchType, ContentAttribute.IsHot))
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    whereString.Append($"AND ({ContentAttribute.Title} LIKE '%{keyword}%') ");
                }
                whereString.Append($" AND {searchType} = '{true}'");
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                var list = TableManager.GetAllLowerAttributeNameList(tableStyle, tableName);
                whereString.Append(list.Contains(searchType.ToLower())
                    ? $"AND ({searchType} LIKE '%{keyword}%') "
                    : $"AND (SettingsXML LIKE '%{searchType}={keyword}%') ");
            }

            whereString.Append(dateString);

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False' ");
            }

            if (isNoDup)
            {
                var sqlString = BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, "MIN(Id)", whereString + " GROUP BY Title");
                whereString.Append($"AND Id IN ({sqlString})");
            }

            if (!string.IsNullOrEmpty(userNameOnly))
            {
                whereString.Append($" AND AddUserName = '{userNameOnly}' ");
            }
            if (isWritingOnly)
            {
                whereString.Append(" AND WritingUserName <> '' ");
            }

            whereString.Append(" ").Append(orderByString);

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetSelectCommendByWhere(string tableName, int publishmentSystemId, List<int> nodeIdList, string where, ETriState checkedState)
        {
            if (nodeIdList == null || nodeIdList.Count == 0)
            {
                return null;
            }

            var orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            var whereString = new StringBuilder("WHERE ");

            whereString.Append(
                nodeIdList.Count == 1
                    ? $"PublishmentSystemId = {publishmentSystemId} AND (NodeId = {nodeIdList[0]}) AND ({where}) "
                    : $"PublishmentSystemId = {publishmentSystemId} AND (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)})) AND ({where}) ");

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False' ");
            }

            whereString.Append(orderByString);

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetSelectCommend(string tableName, int nodeId, ETriState checkedState)
        {
            var orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            var whereString = new StringBuilder();
            whereString.Append($"WHERE (NodeId = {nodeId}) ");

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False'");
            }

            //whereString.Append(orderByString);

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString(), orderByString);
        }

        public string GetSelectCommend(string tableName, int nodeId, ETriState checkedState, string userNameOnly)
        {
            var orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            var whereString = new StringBuilder();
            whereString.Append($"WHERE (NodeId = {nodeId}) ");

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False'");
            }

            if (!string.IsNullOrEmpty(userNameOnly))
            {
                whereString.Append($" AND AddUserName = '{userNameOnly}' ");
            }

            //whereString.Append(orderByString);

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString(), orderByString);
        }

        public string GetSelectCommend(string tableName, List<int> nodeIdList, ETriState checkedState)
        {
            var orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            var whereString = new StringBuilder();

            whereString.Append(nodeIdList.Count == 1
                ? $"WHERE (NodeId = {nodeIdList[0]}) "
                : $"WHERE (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)})) ");

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False'");
            }

            whereString.Append(orderByString);

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetSelectCommendByHitsAnalysis(string tableName, int publishmentSystemId)
        {
            var orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            var whereString = new StringBuilder();
            whereString.Append($"AND IsChecked='{true}' AND PublishmentSystemId = {publishmentSystemId} AND Hits > 0");
            whereString.Append(orderByString);

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetValue(string tableName, int contentId, string name)
        {
            string sqlString = $"SELECT {name} FROM {tableName} WHERE (Id = {contentId})";
            return BaiRongDataProvider.DatabaseDao.GetString(sqlString);
        }

        public void SetValue(string tableName, int contentId, string name, string value)
        {
            string sqlString = $"UPDATE {tableName} SET {name} = '{value}' WHERE Id = {contentId}";

            ExecuteNonQuery(sqlString);
        }

        public void AddContentGroupList(string tableName, int contentId, List<string> contentGroupList)
        {
            var list = TranslateUtils.StringCollectionToStringList(GetValue(tableName, contentId, ContentAttribute.ContentGroupNameCollection));
            foreach (var groupName in contentGroupList)
            {
                if (!list.Contains(groupName)) list.Add(groupName);
            }
            SetValue(tableName, contentId, ContentAttribute.ContentGroupNameCollection, TranslateUtils.ObjectCollectionToString(list));
        }

        public List<int> GetReferenceIdList(string tableName, List<int> contentIdList)
        {
            var arraylist = new List<int>();
            string sqlString =
                $"SELECT Id FROM {tableName} WHERE NodeId > 0 AND ReferenceId IN ({TranslateUtils.ToSqlInStringWithoutQuote(contentIdList)})";

            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(GetInt(rdr, 0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public int GetFirstContentId(string tableName, int nodeId)
        {
            string sqlString = $"SELECT Id FROM {tableName} WHERE NodeId = {nodeId} ORDER BY Taxis DESC, Id DESC";
            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        public List<int> GetContentIdList(string tableName, int nodeId)
        {
            var arraylist = new List<int>();

            string sqlString = $"SELECT Id FROM {tableName} WHERE NodeId = {nodeId}";
            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var contentId = GetInt(rdr, 0);
                    arraylist.Add(contentId);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public List<int> GetContentIdList(string tableName, int nodeId, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState)
        {
            var arraylist = new List<int>();

            string sqlString = $"SELECT Id FROM {tableName} WHERE NodeId = {nodeId}";
            if (isPeriods)
            {
                var dateString = string.Empty;
                if (!string.IsNullOrEmpty(dateFrom))
                {
                    dateString = $" AND AddDate >= '{dateFrom}' ";
                }
                if (!string.IsNullOrEmpty(dateTo))
                {
                    dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                    dateString += $" AND AddDate <= '{dateTo}' ";
                }
                sqlString += dateString;
            }

            if (checkedState != ETriState.All)
            {
                sqlString += $" AND IsChecked = '{ETriStateUtils.GetValue(checkedState)}'";
            }

            sqlString += " ORDER BY Taxis DESC, Id DESC";

            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var contentId = GetInt(rdr, 0);
                    arraylist.Add(contentId);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public List<int> GetContentIdListByPublishmentSystemId(string tableName, int publishmentSystemId)
        {
            var arraylist = new List<int>();

            string sqlString = $"SELECT Id FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId}";
            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var contentId = GetInt(rdr, 0);
                    arraylist.Add(contentId);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public List<int> GetContentIdListChecked(string tableName, List<int> nodeIdList, int totalNum, string orderString, string whereString)
        {
            var arraylist = new List<int>();

            if (nodeIdList == null || nodeIdList.Count == 0)
            {
                return arraylist;
            }

            string sqlString;

            if (totalNum > 0)
            {
                sqlString = SqlUtils.GetTopSqlString(tableName, "Id",
                    nodeIdList.Count == 1
                        ? $"WHERE (NodeId = {nodeIdList[0]} AND IsChecked = '{true}' {whereString})"
                        : $"WHERE (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND IsChecked = '{true}' {whereString})", orderString,
                    totalNum);
            }
            else
            {
                sqlString = nodeIdList.Count == 1
                    ? $"SELECT Id FROM {tableName} WHERE (NodeId = {nodeIdList[0]} AND IsChecked = '{true}' {whereString}) {orderString}"
                    : $"SELECT Id FROM {tableName} WHERE (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND IsChecked = '{true}' {whereString}) {orderString}";
            }

            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var contentId = GetInt(rdr, 0);
                    arraylist.Add(contentId);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public List<int> GetContentIdListByTrash(int publishmentSystemId, string tableName)
        {
            string sqlString =
                $"SELECT Id FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId < 0";
            return BaiRongDataProvider.DatabaseDao.GetIntList(sqlString);
        }

        public int TrashContents(int publishmentSystemId, string tableName, List<int> contentIdList)
        {
            if (!string.IsNullOrEmpty(tableName) && contentIdList != null && contentIdList.Count > 0)
            {
                string sqlString =
                    $"UPDATE {tableName} SET NodeId = -NodeId, LastEditDate = {SqlUtils.GetDefaultDateString()} WHERE PublishmentSystemId = {publishmentSystemId} AND Id IN ({TranslateUtils.ToSqlInStringWithoutQuote(contentIdList)})";
                return ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int TrashContentsByNodeId(int publishmentSystemId, string tableName, int nodeId)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString =
                    $"UPDATE {tableName} SET NodeId = -NodeId, LastEditDate = {SqlUtils.GetDefaultDateString()} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {publishmentSystemId}";
                return ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int DeleteContents(int publishmentSystemId, string tableName, List<int> contentIdList)
        {
            if (!string.IsNullOrEmpty(tableName) && contentIdList != null && contentIdList.Count > 0)
            {
                TagUtils.RemoveTags(publishmentSystemId, contentIdList);

                string sqlString =
                    $"DELETE FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND Id IN ({TranslateUtils.ToSqlInStringWithoutQuote(contentIdList)})";
                return ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int DeleteContentsByNodeId(int publishmentSystemId, string tableName, int nodeId, List<int> contentIdList)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                TagUtils.RemoveTags(publishmentSystemId, contentIdList);

                string sqlString =
                    $"DELETE FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeId}";
                return ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public void DeleteContentsArchive(int publishmentSystemId, string tableName, List<int> contentIdList)
        {
            if (!string.IsNullOrEmpty(tableName) && contentIdList != null && contentIdList.Count > 0)
            {
                string sqlString =
                    $"DELETE FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND Id IN ({TranslateUtils.ToSqlInStringWithoutQuote(contentIdList)})";
                ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteContentsByTrash(int publishmentSystemId, string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                var contentIdList = BaiRongDataProvider.ContentDao.GetContentIdListByTrash(publishmentSystemId, tableName);
                TagUtils.RemoveTags(publishmentSystemId, contentIdList);

                string sqlString =
                    $"DELETE FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId < 0";
                ExecuteNonQuery(sqlString);
            }
        }

        public int RestoreContentsByTrash(int publishmentSystemId, string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString =
                    $"UPDATE {tableName} SET NodeId = -NodeId, LastEditDate = {SqlUtils.GetDefaultDateString()} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId < 0";
                return ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int GetContentId(string tableName, int nodeId, int taxis, bool isNextContent)
        {
            var contentId = 0;
            var sqlString = SqlUtils.GetTopSqlString(tableName, "Id", $"WHERE (NodeId = {nodeId} AND Taxis > {taxis} AND IsChecked = 'True')", "ORDER BY Taxis", 1);
            if (isNextContent)
            {
                sqlString = SqlUtils.GetTopSqlString(tableName, "Id",
                $"WHERE (NodeId = {nodeId} AND Taxis < {taxis} AND IsChecked = 'True')", "ORDER BY Taxis DESC", 1);
            }

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    contentId = GetInt(rdr, 0);
                }
                rdr.Close();
            }
            return contentId;
        }

        //根据排序规则获得第一条内容的Id
        public int GetContentId(string tableName, int nodeId, string orderByString)
        {
            var contentId = 0;
            var sqlString = SqlUtils.GetTopSqlString(tableName, "Id", $"WHERE (NodeId = {nodeId})", orderByString, 1);

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    contentId = GetInt(rdr, 0);
                }
                rdr.Close();
            }
            return contentId;
        }

        public int GetContentId(string tableName, int nodeId, string attributeName, string value)
        {
            var contentId = 0;
            string sqlString = $"SELECT Id FROM {tableName} WHERE (NodeId = {nodeId} AND {attributeName} = '{value}')";

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    contentId = GetInt(rdr, 0);
                }
                rdr.Close();
            }
            return contentId;
        }

        public List<string> GetValueList(string tableName, int nodeId, string name)
        {
            string sqlString = $"SELECT {name} FROM {tableName} WHERE NodeId = {nodeId}";
            return BaiRongDataProvider.DatabaseDao.GetStringList(sqlString);
        }

        public List<string> GetValueListByStartString(string tableName, int nodeId, string name, string startString, int totalNum)
        {
            var inStr = SqlUtils.GetInStr(name, startString);
            var sqlString = SqlUtils.GetDistinctTopSqlString(tableName, name, $"WHERE NodeId = {nodeId} AND {inStr}", string.Empty, totalNum);
            return BaiRongDataProvider.DatabaseDao.GetStringList(sqlString);
        }

        public int GetNodeId(string tableName, int contentId)
        {
            var nodeId = 0;
            string sqlString = $"SELECT {ContentAttribute.NodeId} FROM {tableName} WHERE (Id = {contentId})";

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    nodeId = GetInt(rdr, 0);
                }
                rdr.Close();
            }
            return nodeId;
        }

        public DateTime GetAddDate(string tableName, int contentId)
        {
            var addDate = DateTime.Now;
            string sqlString = $"SELECT {ContentAttribute.AddDate} FROM {tableName} WHERE (Id = {contentId})";

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    addDate = GetDateTime(rdr, 0);
                }
                rdr.Close();
            }
            return addDate;
        }

        public DateTime GetLastEditDate(string tableName, int contentId)
        {
            var lastEditDate = DateTime.Now;
            string sqlString = $"SELECT {ContentAttribute.LastEditDate} FROM {tableName} WHERE (Id = {contentId})";

            using (var rdr = ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lastEditDate = GetDateTime(rdr, 0);
                }
                rdr.Close();
            }
            return lastEditDate;
        }

        public int GetCount(string tableName, int nodeId)
        {
            string sqlString = $"SELECT COUNT(*) AS TotalNum FROM {tableName} WHERE (NodeId = {nodeId})";

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        public virtual int GetCountChecked(string tableName, int nodeId, int days)
        {
            var whereString = string.Empty;
            if (days > 0)
            {
                whereString = "AND " + SqlUtils.GetDateDiffLessThanDays("AddDate", days.ToString());
            }
            return GetCountChecked(tableName, nodeId, whereString);
        }

        public int GetCountChecked(string tableName, int nodeId, string whereString)
        {
            string sqlString =
                $"SELECT COUNT(*) AS TotalNum FROM {tableName} WHERE (NodeId = {nodeId} AND IsChecked = '{true}' {whereString})";

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        public int GetSequence(string tableName, int nodeId, int contentId)
        {
            string sqlString =
                $"SELECT COUNT(*) AS TotalNum FROM {tableName} WHERE NodeId = {nodeId} AND IsChecked = '{true}' AND Taxis < (SELECT Taxis FROM {tableName} WHERE (Id = {contentId}))";

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString) + 1;
        }

        public string GetSelectCommendOfAdminExcludeRecycle(string tableName, int publishmentSystemId, DateTime begin, DateTime end)
        {
            string sqlString = $@"select userName,SUM(addCount) as addCount, SUM(updateCount) as updateCount from( 
SELECT AddUserName as userName, Count(AddUserName) as addCount, 0 as updateCount FROM {tableName} 
INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName 
WHERE {tableName}.PublishmentSystemId = {publishmentSystemId} AND (({tableName}.NodeId > 0)) 
AND LastEditDate BETWEEN '{DateUtils.GetDateString(begin)}' AND '{DateUtils.GetDateString(end.AddDays(1))}'
GROUP BY AddUserName
Union
SELECT LastEditUserName as userName,0 as addCount, Count(LastEditUserName) as updateCount FROM {tableName} 
INNER JOIN bairong_Administrator ON LastEditUserName = bairong_Administrator.UserName 
WHERE {tableName}.PublishmentSystemId = {publishmentSystemId} AND (({tableName}.NodeId > 0)) 
AND LastEditDate BETWEEN '{DateUtils.GetDateString(begin)}' AND '{DateUtils.GetDateString(end.AddDays(1))}'
AND LastEditDate != AddDate
GROUP BY LastEditUserName
) as tmp
group by tmp.userName";


            return sqlString;
        }

        public virtual List<int> GetNodeIdListCheckedByLastEditDateHour(string tableName, int publishmentSystemId, int hour)
        {
            var arraylist = new List<int>();

            string sqlString =
                $"SELECT DISTINCT NodeId FROM {tableName} WHERE (PublishmentSystemId = {publishmentSystemId}) AND (IsChecked = '{true}') AND (LastEditDate BETWEEN '{DateUtils.GetDateAndTimeString(DateTime.Now.AddHours(-hour))}' AND '{DateUtils.GetDateAndTimeString(DateTime.Now)}')";

            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var nodeId = GetInt(rdr, 0);
                    arraylist.Add(nodeId);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public string GetSelectedCommendByCheck(string tableName, int publishmentSystemId, bool isSystemAdministrator, List<int> owningNodeIdList, List<int> checkLevelList)
        {
            string whereString;

            if (isSystemAdministrator)
            {
                whereString =
                    $"WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId > 0 AND IsChecked='{false}' AND CheckedLevel IN ({TranslateUtils.ToSqlInStringWithoutQuote(checkLevelList)}) ";
            }
            else
            {
                whereString = owningNodeIdList.Count == 1 ? $"WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {owningNodeIdList[0]} AND IsChecked='{false}' AND CheckedLevel IN ({TranslateUtils.ToSqlInStringWithoutQuote(checkLevelList)}) " : $"WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(owningNodeIdList)}) AND IsChecked='{false}' AND CheckedLevel IN ({TranslateUtils.ToSqlInStringWithoutQuote(checkLevelList)}) ";
            }

            return BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString);
        }

        public DataSet GetStlDataSourceChecked(ETableStyle tableStyle, string tableName, List<int> nodeIdList, int startNum, int totalNum, string orderByString, string whereString, bool isNoDup, LowerNameValueCollection others)
        {
            if (nodeIdList == null || nodeIdList.Count == 0)
            {
                return null;
            }
            var sqlWhereString = nodeIdList.Count == 1 ? $"WHERE (NodeId = {nodeIdList[0]} AND IsChecked = '{true}' {whereString})" : $"WHERE (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND IsChecked = '{true}' {whereString})";

            if (isNoDup)
            {
                var sqlString = BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, "MIN(Id)", sqlWhereString + " GROUP BY Title");
                sqlWhereString += $" AND Id IN ({sqlString})";
            }

            if (others != null && others.Count > 0)
            {
                var lowerColumnNameList = TableManager.GetAllLowerAttributeNameList(tableStyle, tableName);
                foreach (var attributeName in others.Keys)
                {
                    if (lowerColumnNameList.Contains(attributeName.ToLower()))
                    {
                        var value = others.Get(attributeName);
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Trim();
                            if (StringUtils.StartsWithIgnoreCase(value, "not:"))
                            {
                                value = value.Substring("not:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += $" AND ({attributeName} <> '{value}')";
                                }
                                else
                                {
                                    var collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (var val in collection)
                                    {
                                        sqlWhereString += $" AND ({attributeName} <> '{val}')";
                                    }
                                }
                            }
                            else if (StringUtils.StartsWithIgnoreCase(value, "contains:"))
                            {
                                value = value.Substring("contains:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += $" AND ({attributeName} LIKE '%{value}%')";
                                }
                                else
                                {
                                    var builder = new StringBuilder(" AND (");
                                    var collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (var val in collection)
                                    {
                                        builder.Append($" {attributeName} LIKE '%{val}%' OR ");
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                            else if (StringUtils.StartsWithIgnoreCase(value, "start:"))
                            {
                                value = value.Substring("start:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += $" AND ({attributeName} LIKE '{value}%')";
                                }
                                else
                                {
                                    var builder = new StringBuilder(" AND (");
                                    var collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (var val in collection)
                                    {
                                        builder.Append($" {attributeName} LIKE '{val}%' OR ");
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                            else if (StringUtils.StartsWithIgnoreCase(value, "end:"))
                            {
                                value = value.Substring("end:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += $" AND ({attributeName} LIKE '%{value}')";
                                }
                                else
                                {
                                    var builder = new StringBuilder(" AND (");
                                    var collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (var val in collection)
                                    {
                                        builder.Append($" {attributeName} LIKE '%{val}' OR ");
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                            else
                            {
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += $" AND ({attributeName} = '{value}')";
                                }
                                else
                                {
                                    var builder = new StringBuilder(" AND (");
                                    var collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (var val in collection)
                                    {
                                        builder.Append($" {attributeName} = '{val}' OR ");
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return startNum <= 1 ? GetStlDataSourceByContentNumAndWhereString(tableName, totalNum, sqlWhereString, orderByString) : GetStlDataSourceByStartNum(tableName, startNum, totalNum, sqlWhereString, orderByString);
        }

        public int GetStlCountChecked(string tableName, List<int> nodeIdList, string whereString)
        {
            if (nodeIdList == null || nodeIdList.Count == 0)
            {
                return 0;
            }
            var sqlWhereString = nodeIdList.Count == 1 ? $"WHERE (NodeId ={nodeIdList[0]} AND IsChecked = '{true}' {whereString})" : $"WHERE (NodeId IN ({TranslateUtils.ToSqlInStringWithoutQuote(nodeIdList)}) AND IsChecked = '{true}' {whereString})";

            string sqlString = $"SELECT COUNT(*) FROM {tableName} {sqlWhereString}";

            return BaiRongDataProvider.DatabaseDao.GetIntResult(sqlString);
        }

        private DataSet GetStlDataSourceByContentNumAndWhereString(string tableName, int totalNum, string whereString, string orderByString)
        {
            DataSet dataset = null;
            if (!string.IsNullOrEmpty(tableName))
            {
                var sqlSelect = BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, totalNum, StlColumns, whereString, orderByString);
                dataset = ExecuteDataset(sqlSelect);
            }
            return dataset;
        }

        private DataSet GetStlDataSourceByStartNum(string tableName, int startNum, int totalNum, string whereString, string orderByString)
        {
            DataSet dataset = null;
            if (!string.IsNullOrEmpty(tableName))
            {
                var sqlSelect = BaiRongDataProvider.DatabaseDao.GetSelectSqlString(tableName, startNum, totalNum, StlColumns, whereString, orderByString);
                dataset = ExecuteDataset(sqlSelect);
            }
            return dataset;
        }

        public string StlColumns => $"{ContentAttribute.Id}, {ContentAttribute.NodeId}, {ContentAttribute.IsTop}, {ContentAttribute.AddDate}, {ContentAttribute.LastEditDate}, {ContentAttribute.Taxis}, {ContentAttribute.Hits}, {ContentAttribute.HitsByDay}, {ContentAttribute.HitsByWeek}, {ContentAttribute.HitsByMonth}";

        public string GetStlWhereString(int publishmentSystemId, string group, string groupNot, string tags, bool isTopExists, bool isTop, string where)
        {
            var whereStringBuilder = new StringBuilder();

            if (isTopExists)
            {
                whereStringBuilder.Append($" AND IsTop = '{isTop}' ");
            }

            if (!string.IsNullOrEmpty(group))
            {
                group = group.Trim().Trim(',');
                var groupArr = group.Split(',');
                if (groupArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (var theGroup in groupArr)
                    {
                        //whereStringBuilder.Append(
                        //    $" ({ContentAttribute.ContentGroupNameCollection} = '{theGroup.Trim()}' OR CHARINDEX('{theGroup.Trim()},',{ContentAttribute.ContentGroupNameCollection}) > 0 OR CHARINDEX(',{theGroup.Trim()},',{ContentAttribute.ContentGroupNameCollection}) > 0 OR CHARINDEX(',{theGroup.Trim()}',{ContentAttribute.ContentGroupNameCollection}) > 0) OR ");

                        whereStringBuilder.Append(
                                $" ({ContentAttribute.ContentGroupNameCollection} = '{theGroup.Trim()}' OR {SqlUtils.GetInStr(ContentAttribute.ContentGroupNameCollection, theGroup.Trim() + ",")} OR {SqlUtils.GetInStr(ContentAttribute.ContentGroupNameCollection, "," + theGroup.Trim() + ",")} OR {SqlUtils.GetInStr(ContentAttribute.ContentGroupNameCollection, "," + theGroup.Trim())}) OR ");
                    }
                    if (groupArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 3;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(groupNot))
            {
                groupNot = groupNot.Trim().Trim(',');
                var groupNotArr = groupNot.Split(',');
                if (groupNotArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (var theGroupNot in groupNotArr)
                    {
                        //whereStringBuilder.Append(
                        //    $" ({ContentAttribute.ContentGroupNameCollection} <> '{theGroupNot.Trim()}' AND CHARINDEX('{theGroupNot.Trim()},',{ContentAttribute.ContentGroupNameCollection}) = 0 AND CHARINDEX(',{theGroupNot.Trim()},',{ContentAttribute.ContentGroupNameCollection}) = 0 AND CHARINDEX(',{theGroupNot.Trim()}',{ContentAttribute.ContentGroupNameCollection}) = 0) AND ");

                        whereStringBuilder.Append(
                                $" ({ContentAttribute.ContentGroupNameCollection} <> '{theGroupNot.Trim()}' AND {SqlUtils.GetNotInStr(ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim() + ",")} AND {SqlUtils.GetNotInStr(ContentAttribute.ContentGroupNameCollection, "," + theGroupNot.Trim() + ",")} AND {SqlUtils.GetNotInStr(ContentAttribute.ContentGroupNameCollection, "," + theGroupNot.Trim())}) AND ");
                    }
                    if (groupNotArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 4;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(tags))
            {
                var tagCollection = TagUtils.ParseTagsString(tags);
                var contentIdList = BaiRongDataProvider.TagDao.GetContentIdListByTagCollection(tagCollection, publishmentSystemId);
                if (contentIdList.Count > 0)
                {
                    var inString = TranslateUtils.ToSqlInStringWithoutQuote(contentIdList);
                    whereStringBuilder.Append($" AND (Id IN ({inString}))");
                }
            }

            if (!string.IsNullOrEmpty(where))
            {
                whereStringBuilder.Append($" AND ({where}) ");
            }

            return whereStringBuilder.ToString();
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        public List<int> GetContentIdListCheck(int publishmentSystemId, int nodeId, string tableName)
        {
            var list = new List<int>();

            string sqlString =
                $"SELECT Id FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeId} AND IsChecked = '{false}'";
            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var contentId = GetInt(rdr, 0);
                    list.Add(contentId);
                }
                rdr.Close();
            }
            return list;
        }

        public List<int> GetContentIdArrayListUnCheck(int publishmentSystemId, int nodeId, string tableName)
        {
            var list = new List<int>();

            string sqlString =
                $"SELECT Id FROM {tableName} WHERE PublishmentSystemId = {publishmentSystemId} AND NodeId = {nodeId} AND IsChecked = '{true}'";
            using (var rdr = ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    var contentId = GetInt(rdr, 0);
                    list.Add(contentId);
                }
                rdr.Close();
            }
            return list;
        }

        public DataSet GetDataSetOfAdminExcludeRecycle(string tableName, int publishmentSystemId, DateTime begin, DateTime end)
        {
            var sqlString = GetSelectCommendOfAdminExcludeRecycle(tableName, publishmentSystemId, begin, end);

            return ExecuteDataset(sqlString);
        }
    }
}
