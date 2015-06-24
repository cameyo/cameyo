package com.freerdp.freerdpcore.presentation;

import java.util.HashMap;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;

public abstract class BaseFragment extends Fragment {
	public AppMainActivity mActivity;	
	protected HashMap<String, Object> m_HashObject;
	
	
	@Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);                
    }
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mActivity		=	(AppMainActivity) this.getActivity();
		m_HashObject = mActivity.currentFragmentInfo;
	}
	
	public abstract boolean onBackPressed();
	public abstract boolean onNextPressed();

	@Override
	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);
	}
	
	
}
