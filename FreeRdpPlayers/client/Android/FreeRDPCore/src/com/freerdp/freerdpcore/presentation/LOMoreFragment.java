package com.freerdp.freerdpcore.presentation;

import java.util.ArrayList;

import android.content.Context;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.res.Resources;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.text.Html;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;
import android.text.method.LinkMovementMethod;

import com.freerdp.freerdpcore.R;
import com.freerdp.freerdpcore.manager.AppPreferences;
import com.freerdp.freerdpcore.manager.GlobalAPI;
import com.freerdp.freerdpcore.manager.Server;
import com.freerdp.freerdpcore.model.AppModel;
import com.freerdp.freerdpcore.model.ConnectionModel;
import com.freerdp.freerdpcore.task.BaseTask;
import com.freerdp.freerdpcore.task.TaskListener;

/**
 * User: YRH
 * Date: 11/25/13
 * Time: 2:02 AM
 */
public class LOMoreFragment extends BaseFragment implements TaskListener, OnItemClickListener{

    private GridView mAppBoardGridView;
    private TextView mNoAppLabel, mAddAppLabel;

    private ArrayList<AppModel> mAppArray = new ArrayList<AppModel>();

    private MyAdapter mGridAdapter;

    private String currentPackageId;

    private String currentCategory;

    private static final int TASK_FETCHAPPLIST = 5001;
    private static final int TASK_ESTABLISHCONNECTION = 5002;

    @Override
    public final View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_lo_more, container, false);
        InitView(v);

        return v;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        Log.d(this.getClass().getSimpleName(), "onCreate()");

        super.onCreate(savedInstanceState);

        mActivity.setActionBackFlag(false);
		mActivity.setActionButtonFlag(false);

		if (AppPreferences.currentCategories != null && AppPreferences.currentCategories.length > 0) {
			loadCategory(AppPreferences.currentCategories[0].id);
		} else {

		}
    }



    @Override
	public void onResume() {
		// TODO Auto-generated method stub
		super.onResume();

		Log.d(this.getClass().getSimpleName(), "onResume()");

		if (AppPreferences.currentCategories != null && AppPreferences.currentCategories.length > 0) {

		} else {
			Intent intent = new Intent(mActivity, LoginActivity.class);
			intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
			mActivity.startActivity(intent);
		}
	}

	@Override
	public void onActivityCreated(@Nullable Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onActivityCreated(savedInstanceState);
		Log.d(this.getClass().getSimpleName(), "onActivityCreated()");
	}

	@Override
	public void onStart() {
		// TODO Auto-generated method stub
    	super.onStart();
		Log.d(this.getClass().getSimpleName(), "onStart()");

	}

	private void InitView(View root) {
    	mAppBoardGridView = (GridView) root.findViewById(R.id.app_gridview);

    	boolean tabletSize = getResources().getBoolean(R.bool.isTablet);
		if (tabletSize) {
			mAppBoardGridView.setNumColumns(2);
		} else {
			mAppBoardGridView.setNumColumns(1);
		}

    	mGridAdapter = new MyAdapter(mActivity);
    	mAppBoardGridView.setAdapter(mGridAdapter);
    	mAppBoardGridView.setOnItemClickListener(this);

    	mNoAppLabel = (TextView) root.findViewById(R.id.noapp_textview);
    	mNoAppLabel.setVisibility(View.INVISIBLE);

    	mAddAppLabel = (TextView) root.findViewById(R.id.addapp_textview);
        mAddAppLabel.setVisibility(View.INVISIBLE);
        String addAppTxt = getResources().getString(R.string.addapp_string);
        addAppTxt = "<a href='" + Server.serverUrl("add") + "'>" + addAppTxt + "</a>";
        mAddAppLabel.setText(Html.fromHtml(addAppTxt));
        mAddAppLabel.setMovementMethod(LinkMovementMethod.getInstance());
    }

    public void loadCategory(String category) {
    	currentCategory = category;

    	mActivity.showWaitingDialog();
		BaseTask signupTask = new BaseTask(TASK_FETCHAPPLIST);
		signupTask.setListener(this);
		signupTask.execute();
    }


    @Override
	public boolean onBackPressed() {
		mActivity.popFragments(0);
		return true;
	}

	@Override
	public boolean onNextPressed() {
		return true;
	}

	private class MyAdapter extends BaseAdapter
    {
        private LayoutInflater inflater;

        public MyAdapter(Context context)
        {
            inflater = LayoutInflater.from(context);
        }

        @Override
        public int getCount() {
            return mAppArray.size();
        }

        @Override
        public Object getItem(int i)
        {
            return mAppArray.get(i);
        }

        @Override
        public long getItemId(int i)
        {
            return 0;
        }

        @Override
        public View getView(int i, View view, ViewGroup viewGroup)
        {
            View v = view;
            ImageView picture;
            TextView name;
            TextView desc;
            TextView version;
            TextView history;

            if(v == null)
            {
               v = inflater.inflate(R.layout.appboard_row_item, viewGroup, false);
               v.setTag(R.id.appicon_imageview, v.findViewById(R.id.appicon_imageview));
               v.setTag(R.id.appname_textview, v.findViewById(R.id.appname_textview));
               v.setTag(R.id.appdesc_textview, v.findViewById(R.id.appdesc_textview));
               v.setTag(R.id.apphistory_textview, v.findViewById(R.id.apphistory_textview));
               v.setTag(R.id.appversion_textview, v.findViewById(R.id.appversion_textview));
            }

            picture = (ImageView)v.getTag(R.id.appicon_imageview);
            name = (TextView)v.getTag(R.id.appname_textview);
            desc = (TextView)v.getTag(R.id.appdesc_textview);
            version = (TextView)v.getTag(R.id.appversion_textview);
            history = (TextView)v.getTag(R.id.apphistory_textview);

            AppModel item = (AppModel)getItem(i);

            picture.setImageResource(R.drawable.user_icon);
            GlobalAPI.displayImage(picture, item.iconUrl);
            name.setText(item.appId);
            desc.setText(item.appId);
            String infoStr = item.infoStr;
            if (infoStr != null && infoStr.contains("|")) {
            	String[] splitedStr = infoStr.split("\\|");
            	if (splitedStr.length > 1) {
            		version.setText(splitedStr[0]);
            		history.setText(splitedStr[1]);
            	}
            }
            version.setText(item.infoStr);
            return v;
        }
    }

	@Override
	public Object onTaskRunning(int taskId, Object data) {
		if (taskId == TASK_FETCHAPPLIST) {
			return Server.getAppArray(AppPreferences.getInstance(getActivity()).getLoEmailId(),
					AppPreferences.getInstance(getActivity()).getLoPassword(), currentCategory);
		} else if (taskId == TASK_ESTABLISHCONNECTION) {
			return Server.sendConnectionRequest(AppPreferences.getInstance(getActivity()).getLoEmailId(),
					AppPreferences.getInstance(getActivity()).getLoPassword(), currentPackageId);
		}
		return null;
	}

	@Override
	public void onTaskResult(int taskId, Object result) {
		mActivity.hideWaitingDialog();
		if (taskId == TASK_FETCHAPPLIST) {
			AppModel[] retArray = (AppModel[])result;

			mAppArray.clear();
			if (retArray != null && retArray.length > 0) {
				for (int i = 0; i < retArray.length; i ++) {
					mAppArray.add(retArray[i]);
				}
				mNoAppLabel.setVisibility(View.INVISIBLE);
                mAddAppLabel.setVisibility(View.INVISIBLE);
			} else {
				mNoAppLabel.setVisibility(View.VISIBLE);
                mAddAppLabel.setVisibility(View.VISIBLE);
			}
			mGridAdapter.notifyDataSetChanged();
		} else if (taskId == TASK_ESTABLISHCONNECTION) {
			ConnectionModel retModel = (ConnectionModel)result;
			AppPreferences.getInstance(getActivity()).currentConnection = retModel;

			Bundle bundle = new Bundle();
			bundle.putString(SessionActivity.PARAM_CONNECTION_CAMEYO, "now");
			Intent sessionIntent = new Intent(mActivity, SessionActivity.class);
			sessionIntent.putExtras(bundle);

			startActivity(sessionIntent);
		}
	}

	@Override
	public void onTaskProgress(int taskId, Object progress) {
	}

	@Override
	public void onItemClick(AdapterView<?> adapterView, View view, int pos, long id) {
		currentPackageId = mAppArray.get(pos).pkgId;
		mActivity.showWaitingDialog();
		BaseTask signupTask = new BaseTask(TASK_ESTABLISHCONNECTION);
		signupTask.setListener(this);
		signupTask.execute();
	}
}
