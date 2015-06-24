package com.freerdp.freerdpcore.manager;

import com.freerdp.freerdpcore.model.ConnectionModel;
import com.freerdp.freerdpcore.model.PackageCategoryModel;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;

public class AppPreferences {
	
	private static final String APP_SHARED_PREFS = "cameyoplayer.sharedpreferences";
	
	private static final String CURRENT_LO_USERID = "CURRENTLOUSERID";
	private static final String CURRENT_LO_EMAILID = "CURRENTLOEMAILID";
	private static final String CURRENT_LO_PASSWORD = "CURRENTLOPASSWORD";
	
	public static PackageCategoryModel[] currentCategories; 
	
	private SharedPreferences appSharedPrefs;
	private Editor prefsEditor;
	
	public ConnectionModel currentConnection;
	
	private static AppPreferences _appPref;
	
	public static AppPreferences getInstance(Context context) {
		if (_appPref == null) {
			_appPref = new AppPreferences(context);
		}
		return _appPref;
	}
	
	public AppPreferences(Context context) {
		this.appSharedPrefs = context.getSharedPreferences(APP_SHARED_PREFS, Activity.MODE_PRIVATE);
		this.prefsEditor = appSharedPrefs.edit();
	}
	
	public void setLoUserId(String userid) {
		prefsEditor.putString(CURRENT_LO_USERID, userid);
		prefsEditor.commit();
	}
	
	public String getLoUserId() {
		return appSharedPrefs.getString(CURRENT_LO_USERID, "");
	}
	
	public void setLoEmailId(String email) {
		prefsEditor.putString(CURRENT_LO_EMAILID, email);
		prefsEditor.commit();
	}
	
	public String getLoEmailId() {
		return appSharedPrefs.getString(CURRENT_LO_EMAILID, "");
	}
	
	public void setLoPassword(String password) {
		prefsEditor.putString(CURRENT_LO_PASSWORD, password);
		prefsEditor.commit();
	}
	
	public String getLoPassword() {
		return appSharedPrefs.getString(CURRENT_LO_PASSWORD, "");
	}	
}
