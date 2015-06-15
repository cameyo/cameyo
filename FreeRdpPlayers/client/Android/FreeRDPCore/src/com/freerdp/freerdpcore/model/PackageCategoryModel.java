package com.freerdp.freerdpcore.model;

import org.json.JSONException;
import org.json.JSONObject;

public class PackageCategoryModel {

	public String id;
	public String displayName;
	public String listMode;

	public static final String TAG = "PackageCategoryModel";

	public PackageCategoryModel() {

	}

	public PackageCategoryModel (JSONObject jsonData) {
		try {			
			JSONObject data = jsonData;

			if (data.has("Id")) {
				id = data.getString("Id");
			}
			if (data.has("DisplayName")) {
				displayName = data.getString("DisplayName");
			}
			if (data.has("ListMode")) {
				listMode = data.getString("ListMode");
			}

		} catch (JSONException e) {
			e.printStackTrace();
		}		
	}
}
