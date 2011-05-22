using UnityEngine;
using System.Collections;


public class UIVerticalPanel : UIAbstractContainer
{
	private UISprite _topStrip;
	private UISprite _middleStrip;
	private UISprite _bottomStrip;
	
	private int _topStripHeight;
	private int _bottomStripHeight;
	
	protected bool _sizeToFit = true; // should the panel make itself the proper size to fit?
	public bool sizeToFit { get { return _sizeToFit; } set { _sizeToFit = value; layoutChildren(); } } // relayout when sizeToFit changes

	public override Vector3 position
	{
		get { return clientTransform.position; }
		set
		{
			base.position = value;
			layoutChildren();
		}
	}


	public static UIVerticalPanel create( string topFilename, string middleFilename, string bottomFilename )
	{
		return create( UI.firstToolkit, topFilename, middleFilename, bottomFilename );
	}
	
	
	public static UIVerticalPanel create( UIToolkit manager, string topFilename, string middleFilename, string bottomFilename )
	{
		return new UIVerticalPanel( manager, topFilename, middleFilename, bottomFilename );
	}


	public UIVerticalPanel( UIToolkit manager, string topFilename, string middleFilename, string bottomFilename ) : base( UILayoutType.Vertical )
	{
		var texInfo = manager.textureInfoForFilename( topFilename );
		_topStrip = manager.addSprite( topFilename, 0, 0, 5 );
		_topStrip.parentUIObject = this;
		_topStripHeight = (int)texInfo.frame.height;
		_width = texInfo.frame.width;
		
		_middleStrip = manager.addSprite( middleFilename, 0, _topStripHeight, 5 );
		_middleStrip.parentUIObject = this;
		
		texInfo = manager.textureInfoForFilename( middleFilename );
		_bottomStrip = manager.addSprite( bottomFilename, 0, 0, 5 );
		_bottomStrip.parentUIObject = this;
		_bottomStripHeight = (int)texInfo.frame.height;
	}


	/// <summary>
	/// Override so that we can set the zIndex to be higher than our background sprites
	/// </summary>
	public void addChild( params UISprite[] children )
	{
		foreach( var child in children )
			child.zIndex = this.zIndex + 1;
		
		base.addChild( children );
	}


	/// <summary>
	/// Responsible for laying out the child UISprites
	/// </summary>
	protected override void layoutChildren()
	{
		base.layoutChildren();
		
		// make sure we have enough height to work with.  totalAvailableHeight will be calculated as the minimum required height for the panel
		// if _height is greater than that, we will use it
		var totalAvailableHeight = _topStripHeight + _bottomStripHeight + 1 + _edgeInsets.top + _edgeInsets.bottom;
		if( _height > totalAvailableHeight )
			totalAvailableHeight = (int)_height;

		// now we move our sprites into position.  we have the proper height now so we can use that if sizeToFit
		var leftoverHeight = totalAvailableHeight - _topStripHeight - _bottomStripHeight;
		
		//_middleStrip.setSize( _middleStrip.width, leftoverHeight );
		_middleStrip.localScale = new Vector3( 1, leftoverHeight, 1 );
		_bottomStrip.localPosition = new Vector3( _bottomStrip.localPosition.x, -_height - _bottomStripHeight, _bottomStrip.localPosition.z );
	}

}
