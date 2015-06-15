package com.freerdp.freerdpcore.manager;

import java.util.regex.Pattern;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.util.Patterns;
import android.widget.ImageView;

import com.freerdp.freerdpcore.R;
import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.utils.DiskCacheUtils;
import com.nostra13.universalimageloader.utils.MemoryCacheUtils;

public class GlobalAPI {

	public static String PDFFILENAME = "Prequal.pdf";
	public static ProgressDialog showProgressDialog(Context context) {		
		return ProgressDialog.show(context, null, context.getString(R.string.loadingprogress), true, false);
	}
	
	public static void showDialogAlert(Context context, String msg) {
		AlertDialog.Builder builder = new AlertDialog.Builder(context);
		builder.setMessage(msg)
		       .setCancelable(true)
		       .setPositiveButton("OK", new DialogInterface.OnClickListener() {
		           public void onClick(DialogInterface dialog, int id) {
		           }
		       });
		AlertDialog alert = builder.create();
		alert.show();
	}
	
	public static void showDialogAlert(Context context, int title, int msg) {
		AlertDialog.Builder builder = new AlertDialog.Builder(context);
		builder.setMessage(context.getString(msg))
			   .setTitle(context.getString(title))
		       .setCancelable(true)
		       .setPositiveButton(android.R.string.ok, new DialogInterface.OnClickListener() {
		           public void onClick(DialogInterface dialog, int id) {
		           }
		       });
		AlertDialog alert = builder.create();
		alert.show();
	}
	
	public static boolean validEmail(String email) {
	    Pattern pattern = Patterns.EMAIL_ADDRESS;
	    return pattern.matcher(email).matches();
	}
	
	public static void displayImage(ImageView imgView, String url) {
		DisplayImageOptions options = new DisplayImageOptions.Builder()
		.cacheInMemory(true)
		.cacheOnDisk(true)
		.considerExifParams(true)
		.build();
		
		String imageUrl = url;
//		File imageFile = ImageLoader.getInstance().getDiscCache().get(imageUrl);
//		if (imageFile.exists()) {
//		    imageFile.delete();
//		}
//		MemoryCacheUtils.removeFromCache(imageUrl, ImageLoader.getInstance().getMemoryCache());
//		ImageLoader.getInstance().clearMemoryCache();	
//		ImageLoader.getInstance().clearDiskCache();
		
//		DiskCacheUtils.removeFromCache(imageUrl, ImageLoader.getInstance().getDiskCache());
//		MemoryCacheUtils.removeFromCache(imageUrl, ImageLoader.getInstance().getMemoryCache());
		
		ImageLoader.getInstance().displayImage(imageUrl, 
				imgView, options);
	}
	
	public static String serverAddress(Context context) {
		return context.getResources().getString(R.string.serveraddress);
	}
	
	public static boolean isHttps(Context context) {
		return context.getResources().getBoolean(R.bool.isHttps);
	}
	
	public static int serverPort(Context context) {
		return context.getResources().getInteger(R.integer.serverport);
	}
}
