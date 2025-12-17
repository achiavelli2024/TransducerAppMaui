using System;
using System.IO;
using Android.App;
using Android.Widget;
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace TransducerAppXA
{
    public static class ExportHelper
    {
        /// <summary>
        /// Copia o banco transducer.db3 (pasta interna do app) para a pasta Downloads do dispositivo.
        /// Retorna o caminho destino se OK, ou null em caso de falha / permissão necessária.
        /// Observação: em dispositivos Android 10+ a escrita direta em Downloads pode ser limitada (Scoped Storage).
        /// Este método funciona na maioria dos emuladores e dispositivos em modo Debug.
        /// </summary>
        public static string ExportDatabaseToDownloads(Activity activity)
        {
            try
            {
                var dbName = "transducer.db3";
                // Caminho interno do app (Files)
                var internalPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);

                if (!File.Exists(internalPath))
                {
                    activity.RunOnUiThread(() => Toast.MakeText(activity, "DB não encontrado: " + internalPath, ToastLength.Long).Show());
                    return null;
                }

                // Pasta Downloads pública (pode ser restrita em Android 10+)
                var downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                if (downloadsDir == null)
                {
                    activity.RunOnUiThread(() => Toast.MakeText(activity, "Não foi possível localizar a pasta Downloads.", ToastLength.Long).Show());
                    return null;
                }

                var destPath = System.IO.Path.Combine(downloadsDir.AbsolutePath, dbName);

                // Se targetSdk < 29, solicitar permissão WRITE_EXTERNAL_STORAGE em runtime
                if ((int)Android.OS.Build.VERSION.SdkInt < (int)Android.OS.BuildVersionCodes.Q)
                {
                    if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                    {
                        // solicita permissão ao usuário (assíncrono)
                        ActivityCompat.RequestPermissions(activity, new string[] { Manifest.Permission.WriteExternalStorage }, 1234);
                        activity.RunOnUiThread(() => Toast.MakeText(activity, "Solicitada permissão. Execute novamente após conceder.", ToastLength.Long).Show());
                        return null;
                    }
                }
                else
                {
                    // Android 10+ (Q e superior): a escrita direta pode não ser permitida em alguns dispositivos.
                    // Ainda assim, em muitos dispositivos/emuladores em modo Debug funciona. Se não funcionar,
                    // use alternativa de compartilhamento (intent) ou implementar MediaStore / SAF.
                }

                // Copia o arquivo
                File.Copy(internalPath, destPath, true);

                activity.RunOnUiThread(() => Toast.MakeText(activity, $"DB exportado para: {destPath}", ToastLength.Long).Show());
                return destPath;
            }
            catch (Exception ex)
            {
                activity?.RunOnUiThread(() => Toast.MakeText(activity, "Erro export DB: " + ex.Message, ToastLength.Long).Show());
                return null;
            }
        }
    }
}