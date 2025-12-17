using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Database;
using Android.Provider;
using Android.Net;
using Android.Content.Res;
using Android.Widget;
using Android.App;
using Android;
using Android.Content.PM;
using AndroidX.Core.Content;
using Android.Content;
using Java.IO;

namespace TransducerAppXA
{
    public static class DbExportHelper
    {
        // --- Helpers internos ---

        static bool IsQOrAbove => (int)Android.OS.Build.VERSION.SdkInt >= 29;

        static string MakeFileName(string prefix) => $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

        // Tenta criar/abrir um OutputStream para um arquivo público (Downloads/.../subFolder)
        // Retorna null se não conseguir (caller deve fallback).
        static Stream TryOpenPublicFileStream(Context context, string subFolderName, string fileName)
        {
            try
            {
                if (IsQOrAbove)
                {
                    // Use MediaStore with RELATIVE_PATH (Android 10+)
                    var values = new ContentValues();
                    values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                    values.Put(MediaStore.IMediaColumns.MimeType, "text/csv");

                    // RELATIVE_PATH = Environment.DirectoryDownloads + "/TransducerAppXA/<subFolderName>"
                    string relative = System.IO.Path.Combine(Android.OS.Environment.DirectoryDownloads, "TransducerAppXA", subFolderName);
                    // In MediaStore the relative path should use forward slashes; Android will accept either in many cases.
                    values.Put(MediaStore.IMediaColumns.RelativePath, relative);

                    Android.Net.Uri collection = MediaStore.Downloads.ExternalContentUri;
                    var uri = context.ContentResolver.Insert(collection, values);
                    if (uri == null) return null;

                    var stream = context.ContentResolver.OpenOutputStream(uri);
                    return stream;
                }
                else
                {
                    // Pre-Q: try to write directly to public downloads (requires WRITE_EXTERNAL_STORAGE permission)
                    var downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                    if (downloadsDir == null) return null;

                    var appRoot = new Java.IO.File(downloadsDir, "TransducerAppXA");
                    if (!appRoot.Exists()) appRoot.Mkdirs();

                    var subDir = new Java.IO.File(appRoot, subFolderName);
                    if (!subDir.Exists()) subDir.Mkdirs();

                    var outFile = new Java.IO.File(subDir, fileName);
                    var stream = new FileStream(outFile.AbsolutePath, FileMode.Create, FileAccess.Write);
                    return stream;
                }
            }
            catch
            {
                return null;
            }
        }

        // Fallback: grava no app-specific files (GetExternalFilesDir/Documents/TransducerAppXA/<subFolder>)
        static string EnsureAppPrivateSubfolder(Context context, string subFolderName)
        {
            var ext = context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            var appDir = new Java.IO.File(ext, "TransducerAppXA");
            if (!appDir.Exists()) appDir.Mkdirs();
            var sub = new Java.IO.File(appDir, subFolderName);
            if (!sub.Exists()) sub.Mkdirs();
            return sub.AbsolutePath;
        }

        // --- Export Results ---

        public static string ExportResultsToCsv(Context context, DbHelper db)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (db == null) throw new ArgumentNullException(nameof(db));

            List<ResultEntry> results;
            try
            {
                results = db.GetAllResults();
            }
            catch
            {
                results = db.GetRecentResults(100000);
            }

            string fileName = MakeFileName("TransducerResults");

            // 1) Tenta gravar em Downloads/TransducerAppXA/Results (public)
            using (var stream = TryOpenPublicFileStream(context, "Results", fileName))
            {
                if (stream != null)
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteLine("Id;TimestampUtc;Torque;Angle;Text");
                        foreach (var r in results)
                        {
                            string ts = r.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            string text = (r.Text ?? "").Replace(";", ",");
                            writer.WriteLine($"{r.Id};{ts};{r.Torque:F3};{r.Angle:F2};{text}");
                        }
                    }

                    // Retornamos um hint público (usa path relativo para indicar Localização na Downloads)
                    string publicPathHint = System.IO.Path.Combine("Downloads", "TransducerAppXA", "Results", fileName);
                    return publicPathHint;
                }
            }

            // Fallback para private
            string baseDirPrivate = EnsureAppPrivateSubfolder(context, "Results");
            string fullPathPrivate = System.IO.Path.Combine(baseDirPrivate, fileName);

            using (var stream = new FileStream(fullPathPrivate, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine("Id;TimestampUtc;Torque;Angle;Text");
                foreach (var r in results)
                {
                    string ts = r.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    string text = (r.Text ?? "").Replace(";", ",");
                    writer.WriteLine($"{r.Id};{ts};{r.Torque:F3};{r.Angle:F2};{text}");
                }
            }

            return fullPathPrivate;
        }

        // --- Export Logs ---
        public static string ExportLogsToCsv(Context context, DbHelper db)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (db == null) throw new ArgumentNullException(nameof(db));

            var logs = db.GetRecentLogs(2000);
            string fileName = MakeFileName("TransducerLogs");

            using (var stream = TryOpenPublicFileStream(context, "Logs", fileName))
            {
                if (stream != null)
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteLine("Id;TimestampUtc;Message");
                        foreach (var l in logs)
                        {
                            string ts = l.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                            string msg = (l.Message ?? "").Replace(";", ",");
                            writer.WriteLine($"{l.Id};{ts};{msg}");
                        }
                    }

                    string publicPathHint = System.IO.Path.Combine("Downloads", "TransducerAppXA", "Logs", fileName);
                    return publicPathHint;
                }
            }

            string baseDirPrivate = EnsureAppPrivateSubfolder(context, "Logs");
            string fullPathPrivate = System.IO.Path.Combine(baseDirPrivate, fileName);

            using (var stream = new FileStream(fullPathPrivate, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                writer.WriteLine("Id;TimestampUtc;Message");
                foreach (var l in logs)
                {
                    string ts = l.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    string msg = (l.Message ?? "").Replace(";", ",");
                    writer.WriteLine($"{l.Id};{ts};{msg}");
                }
            }

            return fullPathPrivate;
        }

        // --- Share/Open via FileProvider / MediaStore fallback ---

        // Tenta localizar um Uri no MediaStore para o public hint (Downloads/TransducerAppXA/...)
        static Android.Net.Uri TryFindMediaStoreUriForPublicHint(Context ctx, string publicHint)
        {
            try
            {
                if (ctx == null || string.IsNullOrWhiteSpace(publicHint)) return null;
                string fileName = System.IO.Path.GetFileName(publicHint);
                if (string.IsNullOrWhiteSpace(fileName)) return null;

                // Query MediaStore.Downloads by DISPLAY_NAME. On Q+ relative path is available too.
                var collection = MediaStore.Downloads.ExternalContentUri;
                string[] projection = new string[] { MediaStore.MediaColumns.Id, MediaStore.MediaColumns.RelativePath, MediaStore.MediaColumns.DisplayName };
                string selection = MediaStore.MediaColumns.DisplayName + "=?";
                string[] selectionArgs = new string[] { fileName };

                using (ICursor cursor = ctx.ContentResolver.Query(collection, projection, selection, selectionArgs, null))
                {
                    if (cursor != null && cursor.MoveToFirst())
                    {
                        int idIndex = cursor.GetColumnIndex(MediaStore.MediaColumns.Id);
                        long id = cursor.GetLong(idIndex);
                        var uri = Android.Content.ContentUris.WithAppendedId(collection, id);
                        return uri;
                    }
                }
            }
            catch (Exception) { /* ignore */ }
            return null;
        }

        // Main share/open function — agora suporta public hint procurando o Uri no MediaStore
        public static void ShareOrOpenCsvFile(Activity activity, string fullPath, bool share = true)
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity));
            if (string.IsNullOrWhiteSpace(fullPath)) throw new ArgumentNullException(nameof(fullPath));

            try
            {
                // Case 1: public hint produced by earlier Export* (e.g. "Downloads/TransducerAppXA/Logs/filename.csv")
                bool looksLikePublicHint = fullPath.StartsWith("Downloads" + System.IO.Path.DirectorySeparatorChar) || fullPath.StartsWith("Downloads/");

                if (looksLikePublicHint)
                {
                    // Try to find the actual MediaStore Uri (Android Q+)
                    var msUri = TryFindMediaStoreUriForPublicHint(activity, fullPath);
                    if (msUri != null)
                    {
                        try
                        {
                            if (share)
                            {
                                var intent = new Intent(Intent.ActionSend);
                                intent.SetType("text/csv");
                                intent.PutExtra(Intent.ExtraSubject, "Transducer CSV");
                                intent.PutExtra(Intent.ExtraStream, msUri);
                                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                                var clip = ClipData.NewUri(activity.ContentResolver, "CSV", msUri);
                                intent.ClipData = clip;

                                activity.StartActivity(Intent.CreateChooser(intent, "Share CSV"));
                                return;
                            }
                            else
                            {
                                var intent = new Intent(Intent.ActionView);
                                intent.SetDataAndType(msUri, "text/csv");
                                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                                var clip = ClipData.NewUri(activity.ContentResolver, "CSV", msUri);
                                intent.ClipData = clip;

                                activity.StartActivity(Intent.CreateChooser(intent, "Open CSV"));
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(activity, "Unable to open or share file via MediaStore: " + ex.Message, ToastLength.Short).Show();
                            return;
                        }
                    }
                    else
                    {
                        // If we couldn't find the file in MediaStore, instruct user to use Files app or Share from file manager.
                        Toast.MakeText(activity, "File saved in public Downloads (use SHARE to send it).", ToastLength.Short).Show();
                        return;
                    }
                }

                // Case 2: absolute file path (private app storage or absolute path)
                var file = new Java.IO.File(fullPath);
                if (!file.Exists())
                {
                    Toast.MakeText(activity, "File not found.", ToastLength.Short).Show();
                    return;
                }

                string authority = activity.PackageName + ".fileprovider";

                try
                {
                    Android.Net.Uri uri = FileProvider.GetUriForFile(activity, authority, file);

                    if (share)
                    {
                        var intent = new Intent(Intent.ActionSend);
                        intent.SetType("text/csv");
                        intent.PutExtra(Intent.ExtraSubject, "Transducer CSV");
                        intent.PutExtra(Intent.ExtraStream, uri);
                        intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                        var clip = ClipData.NewUri(activity.ContentResolver, "CSV", uri);
                        intent.ClipData = clip;

                        activity.StartActivity(Intent.CreateChooser(intent, "Share CSV"));
                    }
                    else
                    {
                        var intent = new Intent(Intent.ActionView);
                        intent.SetDataAndType(uri, "text/csv");
                        intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                        var clip = ClipData.NewUri(activity.ContentResolver, "CSV", uri);
                        intent.ClipData = clip;

                        activity.StartActivity(Intent.CreateChooser(intent, "Open CSV"));
                    }
                }
                catch (Exception ex)
                {
                    // final fallback
                    Toast.MakeText(activity, "Unable to open or share file.", ToastLength.Short).Show();
                }
            }
            catch (Exception)
            {
                Toast.MakeText(activity, "Unable to open or share file.", ToastLength.Short).Show();
            }
        }

        public static void ShareCsvFile(Activity activity, string fullPath) => ShareOrOpenCsvFile(activity, fullPath, true);
        public static void OpenCsvFile(Activity activity, string fullPath) => ShareOrOpenCsvFile(activity, fullPath, false);
    }
}