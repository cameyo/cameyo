package com.freerdp.freerdpcore.presentation;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Stack;

import net.simonvt.menudrawer.MenuDrawer;
import net.simonvt.menudrawer.Position;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.graphics.Color;
import android.graphics.Point;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.util.Log;
import android.view.Display;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.SeekBar;
import android.widget.TextView;

import com.freerdp.freerdpcore.R;
import com.freerdp.freerdpcore.manager.AppPreferences;
import com.freerdp.freerdpcore.manager.GlobalAPI;
import com.freerdp.freerdpcore.model.PackageCategoryModel;
import com.nostra13.universalimageloader.cache.memory.impl.LruMemoryCache;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;

public class AppMainActivity extends FragmentActivity {
	/* A HashMap of stacks, where we use tab identifier as keys..*/
	private HashMap<String, Stack<HashMap<String, Object>>> mNavigatorStacks;
	
	public HashMap<String, Object> currentFragmentInfo;
	
	private BaseFragment m_CurrentFragment;

	/*Save current tabs identifier in this..*/
	private String mCurrentTab;
	
	private ProgressDialog progressDialog;
	
	private boolean isFirstTabLaunch;
	
	private boolean isActionBarEnabled;
	public int currentNextIconId = android.R.drawable.ic_menu_send;
	
	private static final String STATE_ACTIVE_POSITION = "net.simonvt.menudrawer.samples.ContentSample.activePosition";
    private static final String STATE_CONTENT_TEXT = "net.simonvt.menudrawer.samples.ContentSample.contentText";

    private MenuDrawer mMenuDrawer;

    private MenuAdapter mAdapter;
    private ListView mList;
    
    private int mActivePosition = -1;
    private String mContentText;
//    private TextView mContentTextView;

	protected void onCreate(Bundle inState) {
		super.onCreate(inState);
		
		Log.i("Create", "onCreate APPMainActivity");
		getWindow().requestFeature(Window.FEATURE_ACTION_BAR);
		
//		setContentView(R.layout.app_main_tab_fragment_layout);
		
		/*  
		 *  Navigation stacks for each tab gets created.. 
		 *  tab identifier is used as key to get respective stack for each tab
		 */

		mNavigatorStacks = new HashMap<String, Stack<HashMap<String,Object>>>();
		mNavigatorStacks.put(AppConstants.FMENU_MYAPP_TAG, new Stack<HashMap<String,Object>>());
		mNavigatorStacks.put(AppConstants.FMENU_GRAPHICS_TAG, new Stack<HashMap<String,Object>>());
		mNavigatorStacks.put(AppConstants.FMENU_READER_TAG, new Stack<HashMap<String,Object>>());
		mNavigatorStacks.put(AppConstants.FMENU_COMMUNICATION_TAG, new Stack<HashMap<String,Object>>());		

		ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(this)
		        .diskCacheExtraOptions(480, 800, null)
		        .denyCacheImageMultipleSizesInMemory()
		        .memoryCache(new LruMemoryCache(2 * 1024 * 1024))
		        .memoryCacheSize(2 * 1024 * 1024)
		        .diskCacheSize(50 * 1024 * 1024)
		        .diskCacheFileCount(1000)
		        .writeDebugLogs()
		        .build();
		ImageLoader.getInstance().init(config);
		
		if (inState != null) {
            mActivePosition = inState.getInt(STATE_ACTIVE_POSITION);
            mContentText = inState.getString(STATE_CONTENT_TEXT);
        }

		// Use actual screen orientation in order to avoid application crash on activity recreation after setRequestedOrientation.
		//boolean tabletSize = getResources().getBoolean(R.bool.isTablet);
		boolean tabletSize = getResources().getConfiguration().orientation == android.content.res.Configuration.ORIENTATION_LANDSCAPE;
		if (tabletSize) {
		        setRequestedOrientation (ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
			mMenuDrawer = MenuDrawer.attach(this, MenuDrawer.MENU_DRAG_CONTENT, Position.LEFT, true);
			
			Display display = getWindowManager().getDefaultDisplay();
			Point size = new Point();
			display.getSize(size);
			int width = size.x;
			int height = size.y;
			
			mMenuDrawer.setMenuSize(width/4);
		} else {
		        setRequestedOrientation (ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
			mMenuDrawer = MenuDrawer.attach(this, MenuDrawer.MENU_DRAG_CONTENT);
		}
		
		if (AppPreferences.getInstance(this).currentCategories != null && 
				AppPreferences.getInstance(this).currentCategories.length > 1) {
			mMenuDrawer.setContentView(R.layout.app_main_tab_fragment_layout);
			mMenuDrawer.setTouchMode(MenuDrawer.TOUCH_MODE_FULLSCREEN);

			LinearLayout menuView = (LinearLayout)getLayoutInflater().inflate(R.layout.menu_layout, null);

			mList = (ListView) menuView.findViewById(R.id.menu_listview);
			mAdapter = new MenuAdapter(AppPreferences.currentCategories);
			mList.setAdapter(mAdapter);
			mList.setOnItemClickListener(mItemClickListener);
			mList.setDividerHeight(0);

			TextView userEmailText = (TextView) menuView.findViewById(R.id.title_textview);
			userEmailText.setText(AppPreferences.getInstance(this).getLoEmailId());

			mMenuDrawer.setMenuView(menuView);

			mMenuDrawer.setOnInterceptMoveEventListener(new MenuDrawer.OnInterceptMoveEventListener() {
				@Override
				public boolean isViewDraggable(View v, int dx, int x, int y) {
					return v instanceof SeekBar;
				}
			});
		} else {
			setContentView(R.layout.app_main_tab_fragment_layout);
			
			Intent intent = new Intent(this, LoginActivity.class); 
			intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
			startActivity(intent);
			return;
		}
		
		HashMap<String, Object> hashInfo = new HashMap<String, Object>();
		hashInfo.put(AppConstants.LOFRAGMENT_TAG, AppConstants.FMENU_MYAPP_TAG);
		pushFragments(AppConstants.FMENU_MYAPP_TAG, hashInfo, false, true);
		mCurrentTab = AppConstants.FMENU_MYAPP_TAG;
		mActivePosition = 0;
		
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.ICE_CREAM_SANDWICH) {
            getActionBar().setDisplayHomeAsUpEnabled(true);
            getActionBar().setBackgroundDrawable(getResources().getDrawable(R.drawable.navbar));            
        }
	}
	
	
	
	@Override
	protected void onResume() {
		super.onResume();
		Log.i("onResume", "onResume APPMainActivity");
	}



	private void setCategoryIndex(int index) {
		mActivePosition = index;
		if (AppPreferences.currentCategories.length > index) {
			((LOMoreFragment)m_CurrentFragment).loadCategory(AppPreferences.currentCategories[index].id);
		}
	}
	
	private AdapterView.OnItemClickListener mItemClickListener = new AdapterView.OnItemClickListener() {
        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
            setCategoryIndex(position);
            mMenuDrawer.setActiveView(view, position);
            mMenuDrawer.closeMenu();
            mAdapter.notifyDataSetInvalidated();
        }
    };

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);
        outState.putInt(STATE_ACTIVE_POSITION, mActivePosition);
        outState.putString(STATE_CONTENT_TEXT, mContentText);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                mMenuDrawer.toggleMenu();
                return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onBackPressed() {
    	
    	if(mNavigatorStacks.get(mCurrentTab).size() == 1){
    		final int drawerState = mMenuDrawer.getDrawerState();
            if (drawerState == MenuDrawer.STATE_OPEN || drawerState == MenuDrawer.STATE_OPENING) {
                mMenuDrawer.closeMenu();
                return;
            }

            super.onBackPressed();
		}else{		
			m_CurrentFragment.onBackPressed();
		}   
    }


	private View createTabView(final int iconId, final int captionId) {
//		View view = LayoutInflater.from(this).inflate(R.layout.tabs_icon, null);
//		ImageView imageView =   (ImageView) view.findViewById(R.id.tab_icon);
//		imageView.setImageDrawable(getResources().getDrawable(iconId));
//		
//		TextView titleView = (TextView) view.findViewById(R.id.tab_title);
//		titleView.setText(captionId);
		return null;
	}
	
	public BaseFragment prepareCurrentFragment(HashMap<String, Object> hashInfo) {
		BaseFragment currentFragment = new LOMoreFragment();
		currentFragmentInfo = hashInfo;
		String fragmentId = (String) hashInfo.get(AppConstants.LOFRAGMENT_TAG);
		if (fragmentId.equals(AppConstants.MYAPP_APPBOARD_FRAGMENT)) {
			currentFragment = new LOMoreFragment();
		}
		return currentFragment;
	}
	
	public void pushFragments(String tabTag, HashMap<String, Object> hashInfo, boolean shouldAnimate, boolean shouldAdd){
		
		BaseFragment currentFragment = prepareCurrentFragment(hashInfo);
		
		if(shouldAdd)
			mNavigatorStacks.get(tabTag).push(hashInfo);
		
		FragmentManager   manager         =   getSupportFragmentManager();
		FragmentTransaction ft            =   manager.beginTransaction();
//		if(shouldAnimate)
//			ft.setCustomAnimations(R.anim.slide_in_right, R.anim.slide_out_left);
		ft.replace(R.id.realtabcontent, currentFragment);
		ft.commit();
		m_CurrentFragment = currentFragment;
	}

	public void popFragments(int popAdditionalStep){
		/*    
		 *    Select the second last fragment in current tab's stack.. 
		 *    which will be shown after the fragment transaction given below 
		 */
		HashMap<String, Object> fragmentInfo = mNavigatorStacks.get(mCurrentTab).elementAt(mNavigatorStacks.get(mCurrentTab).size() - 2 - popAdditionalStep);
		/*pop current fragment from stack.. */
		for (int i = 0; i < popAdditionalStep + 1; i ++) {
			mNavigatorStacks.get(mCurrentTab).pop();
		}
		
		BaseFragment currentFragment = prepareCurrentFragment(fragmentInfo);

		/* We have the target fragment in hand.. Just show it.. Show a standard navigation animation*/
		FragmentManager   manager         =   getSupportFragmentManager();
		FragmentTransaction ft            =   manager.beginTransaction();
//		ft.setCustomAnimations(R.anim.slide_in_left, R.anim.slide_out_right);
		ft.replace(R.id.realtabcontent, currentFragment);
		ft.commit();
		m_CurrentFragment = currentFragment;
	}

	/*
	 *   Imagine if you wanted to get an image selected using ImagePicker intent to the fragment. Ofcourse I could have created a public function
	 *  in that fragment, and called it from the activity. But couldn't resist myself.
	 */
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if(m_CurrentFragment == null){
			return;
		}

		/*Now current fragment on screen gets onActivityResult callback..*/
		m_CurrentFragment.onActivityResult(requestCode, resultCode, data);
	}
	
	public void showWaitingDialog() {
		progressDialog = GlobalAPI.showProgressDialog(this);
	}
	
	public void hideWaitingDialog() {
		progressDialog.dismiss();
	}
	
	
	public void setActionBackFlag(boolean flag) {
//		getActionBar().setDisplayHomeAsUpEnabled(flag);			
	}
	
	public void setActionButtonFlag(boolean flag) {
		isActionBarEnabled = flag;
//		invalidateOptionsMenu();		
	}
	
	private class MenuAdapter extends BaseAdapter {

        private List<PackageCategoryModel> mItems = new ArrayList<PackageCategoryModel>();

        MenuAdapter(PackageCategoryModel[] categoryArray) {
        	if (categoryArray == null || categoryArray.length > 0) {
        		for (int i = 0; i < categoryArray.length; i ++) {
        			mItems.add(categoryArray[i]);
        		}
        	}            
        }

        @Override
        public int getCount() {
            return mItems.size();
        }

        @Override
        public Object getItem(int position) {
            return mItems.get(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public boolean areAllItemsEnabled() {
            return false;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            View v = convertView;
            PackageCategoryModel item = (PackageCategoryModel) getItem(position);

            if (item instanceof PackageCategoryModel) {
            	if (v == null) {
                    v = getLayoutInflater().inflate(R.layout.menu_row_item, parent, false);
                }

                TextView tv = (TextView) v.findViewById(R.id.title_textview);
                tv.setText(((PackageCategoryModel) item).displayName);                
            }

            v.setTag(R.id.mdActiveViewPosition, position);

            View separtor = (View) v.findViewById(R.id.separator_view);
            if (position == mActivePosition) {
                mMenuDrawer.setActiveView(v, position);
                v.setBackgroundColor(getResources().getColor(R.color.activemenuitem_background));
                separtor.setVisibility(View.INVISIBLE);
            } else {
            	v.setBackgroundColor(Color.TRANSPARENT);
            	separtor.setVisibility(View.VISIBLE);
            }

            return v;
        }
    }
	
}
