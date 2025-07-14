/*
 * Copyright (c). 2024 - 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */
#define NoCameraPerspective

using Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmView3D																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// 3D previewer dialog.
	/// </summary>
	/// <remarks>
	/// <para>
	/// In this version of the viewer, the camera will always be looking at the
	/// origin.
	/// </para>
	/// <para>
	/// This version of the viewer uses Viewport3D's default orientation of
	/// 'Y-up' (photographer's perspective), as opposed to 'Z-up' (drafter's
	/// perspective). As a result, all Y and Z references within this
	/// context are transposed with regard to all other sections of this
	/// project, which use 'Z-up' orientation.
	/// </para>
	/// </remarks>
	public partial class frmView3D : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Value indicating whether the form has been activated at least once
		/// during this instance.
		/// </summary>
		private bool mActivated = false;
		/// <summary>
		/// Value representing the count of individual frames per keyframe.
		/// </summary>
		private int mAnimationFrameCount = 10;
		/// <summary>
		/// Current index between keyframes.
		/// </summary>
		private int mAnimationFrameIndex = 0;
		/// <summary>
		/// Value indicating the total number of available animation keyframes.
		/// </summary>
		private int mAnimationKeyframeCount = 0;
		/// <summary>
		/// Value indicating the current active animation keyframe.
		/// </summary>
		private int mAnimationKeyframeIndex = 0;
		/// <summary>
		/// Reference to the active layer being animated.
		/// </summary>
		private TrackViewLayerItem mAnimationKeyframeLayer = null;
		/// <summary>
		/// Reference to the active track segment being animated.
		/// </summary>
		private TrackViewSegmentItem mAnimationKeyframeSegment = null;
		/// <summary>
		/// The total time index of the current form timer.
		/// </summary>
		private int mAnimationTimeIndex = 0;
#if CameraPerspective
		/// <summary>
		/// The local 3D camera, always looking at the origin and able to orbit
		/// around. The plane of the table is always at the height of 0.
		/// </summary>
		private Camera3D mCamera = new Camera3D()
		{
			LookAt = new FVector3(0f, 0f, 0f),
			UpAxis = AxisType.Z,
			Handedness = HandType.Right
		};
#else
		/// <summary>
		/// The local 3D camera, always looking at the origin and able to orbit
		/// around. The plane of the table is always at the height of 0.
		/// </summary>
		private CameraOrtho mCamera = new CameraOrtho()
		{
			LookAt = new FVector3(0f, 0f, 0f),
			UpAxis = AxisType.Z,
			Handedness = HandType.Right
		};
#endif
		/// <summary>
		/// Value indicating whether the underlying data is being processed.
		/// </summary>
		private bool mDataBusy = false;
		/// <summary>
		/// The current elevation index of the camera.
		/// </summary>
		private int mElevationIndex = 10;
		/// <summary>
		/// The number of elevation indices between 0 and 80 degrees.
		/// </summary>
		private int mElevationPathCount = 160;
		///// <summary>
		///// The last-known mouse position.
		///// </summary>
		//private SKPoint mLastMousePosition = default(SKPoint);
		/// <summary>
		/// The polygon upon which the material is built.
		/// </summary>
		private List<FVector3> mMaterialPolygon = new List<FVector3>();
		///// <summary>
		///// Value indicating whether the mouse button is down.
		///// </summary>
		//private bool mMouseDown = false;
		/// <summary>
		/// The last-known X position of the mouse.
		/// </summary>
		int mMouseLastX = 0;
		/// <summary>
		/// The last-known Y position of the mouse.
		/// </summary>
		int mMouseLastY = 0;
		/// <summary>
		/// A value indicating whether multiple tools are declared.
		/// </summary>
		bool mMultipleToolsDeclared = false;
		/// <summary>
		/// The index of the orbit along its dolly path.
		/// </summary>
		private int mOrbitIndex = 0;
		/// <summary>
		/// Count of orbit index positions in a single orbit.
		/// </summary>
		private int mOrbitPathCount = 720;
		///// <summary>
		///// A list of precalculated points constituting the camera's path
		///// during an orbit.
		///// </summary>
		//private List<FVector2> mOrbitPath = null;
		/// <summary>
		/// The radius of the orbit camera.
		/// </summary>
		private float mOrbitRadius = 0f;
		/// <summary>
		/// The current pitch of the orbit view.
		/// </summary>
		private float mPitch = 0f;
		/// <summary>
		/// The play state of the tool path.
		/// </summary>
		private PlayStateEnum mPlayState = PlayStateEnum.None;
		/// <summary>
		/// The X lines of the grid on the table.
		/// </summary>
		private List<FLine3> mTableGridX = new List<FLine3>();
		/// <summary>
		/// The Y lines of the grid on the table.
		/// </summary>
		private List<FLine3> mTableGridY = new List<FLine3>();
		/// <summary>
		/// The polygon upon which the table is built.
		/// </summary>
		private List<FVector3> mTablePolygon = new List<FVector3>();
		/// <summary>
		/// General state timer.
		/// </summary>
		private Timer mTimerState = new Timer()
		{
			Interval = 50
		};
		///// <summary>
		///// Current drawing mode for track display.
		///// </summary>
		//private TrackDrawModeEnum mTrackDrawMode = TrackDrawModeEnum.DrawAllTracks;
		/// <summary>
		/// Reference to the collection of track layers calculated for this
		/// preview.
		/// </summary>
		private TrackViewLayerCollection mTrackLayers = null;
		/// <summary>
		/// Reference to the underlying raw track layers.
		/// </summary>
		TrackLayerCollection mTracks = null;
		/// <summary>
		/// Value indicating whether the turntable is active.
		/// </summary>
		private bool mTurntable = true;
		/// <summary>
		/// The current yaw of the orbit view.
		/// </summary>
		private float mYaw = 0f;
		///// <summary>
		///// The amount of exaggeration to apply to movements on the Z axis.
		///// </summary>
		//private float mZMagnification = 10f;
		/// <summary>
		/// The current zoom level.
		/// </summary>
		private float mZoom = 1f;
		//private SKMatrix44 mViewMatrix = SKMatrix44.CreateIdentity();

		//*-----------------------------------------------------------------------*
		//* AdjustZoom																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Adjust the current zoom level.
		/// </summary>
		/// <param name="zoomDelta">
		/// Change in zoom.
		/// </param>
		public void AdjustZoom(float zoomDelta)
		{
			// Prevent zooming too close.
			mZoom = Math.Max(0.1f, mZoom + zoomDelta);
			//UpdateViewMatrix();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnCancel_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Cancel button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnFirst_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Index to the first movement.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnFirst_Click(object sender, EventArgs e)
		{
			mAnimationFrameIndex = 0;
			mAnimationKeyframeIndex = 0;
			mPlayState = PlayStateEnum.Pause;
			UpdateKeyframe();
			UpdatePlayState();
			Status("First keyframe.");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnLast_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Index to the last movement.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnLast_Click(object sender, EventArgs e)
		{
			if(mAnimationKeyframeCount > 0)
			{
				mAnimationKeyframeIndex = mAnimationKeyframeCount - 1;
				mAnimationFrameIndex = mAnimationFrameCount - 1;
			}
			else
			{
				mAnimationKeyframeIndex = 0;
				mAnimationFrameIndex = 0;
			}
			mPlayState = PlayStateEnum.Pause;
			UpdateKeyframe();
			UpdatePlayState();
			Status("Last keyframe.");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnNext_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Index to the next movement.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnNext_Click(object sender, EventArgs e)
		{
			if(mAnimationKeyframeIndex + 1 < mAnimationKeyframeCount)
			{
				mAnimationKeyframeIndex++;
				mAnimationFrameIndex = 0;
			}
			else
			{
				mAnimationFrameIndex = mAnimationFrameCount - 1;
			}
			mPlayState = PlayStateEnum.Pause;
			UpdateKeyframe();
			UpdatePlayState();
			Status("Next keyframe.");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOK_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The OK button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnPause_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Pause the animation transport.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnPause_Click(object sender, EventArgs e)
		{
			if(mPlayState == PlayStateEnum.Pause)
			{
				mPlayState = PlayStateEnum.Play;
				Status("Playing animation.");
			}
			else
			{
				mPlayState = PlayStateEnum.Pause;
				Status("Animation paused.");
			}
			UpdatePlayState();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnPlay_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Play from the current location.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnPlay_Click(object sender, EventArgs e)
		{
			mPlayState = PlayStateEnum.Play;
			UpdatePlayState();
			Status("Playing animation.");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnPrevious_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Index to the previous action.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnPrevious_Click(object sender, EventArgs e)
		{
			if(mAnimationKeyframeIndex > 0)
			{
				if(mAnimationKeyframeIndex + 1 == mAnimationKeyframeCount &&
					mAnimationFrameIndex + 1 == mAnimationFrameCount)
				{
					//	Back the tool off of the last position.
					mAnimationFrameIndex = 0;
				}
				else
				{
					mAnimationKeyframeIndex--;
					mAnimationFrameIndex = 0;
					UpdateKeyframe();
				}
			}
			mPlayState = PlayStateEnum.Pause;
			UpdatePlayState();
			Status("Previous keyframe.");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnStop_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Stop the animation and reset the pointer.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnStop_Click(object sender, EventArgs e)
		{
			mPlayState = PlayStateEnum.Stop;
			UpdatePlayState();
			Status("Animation stopped.");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* chkHideTracksUntilVisited_CheckedChanged															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The state of the checkbox indicating whether to hide tracks until
		/// visited has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void chkHideTracksUntilVisited_CheckedChanged(object sender,
			EventArgs e)
		{
			if(chkHideTracksUntilVisited.Checked)
			{
				mTrackLayers.HideAllSegments();
				if(mAnimationKeyframeSegment != null)
				{
					mAnimationKeyframeSegment.Visible = true;
					mTrackLayers.ShowSegmentsBefore(mAnimationKeyframeSegment);
				}
			}
			else
			{
				mTrackLayers.ShowAllSegments();
			}
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* chkTurntable_CheckedChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The state of the turntable checkbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void chkTurntable_CheckedChanged(object sender, EventArgs e)
		{
			mTurntable = chkTurntable.Checked;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DecrementKeyframeIndex																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Decrement the current animation keyframe index.
		/// </summary>
		private void DecrementKeyframeIndex()
		{
			if(mAnimationKeyframeIndex > 0)
			{
				mAnimationFrameIndex = 0;
				mAnimationKeyframeIndex--;
				UpdateKeyframe();
			}
			else if(mPlayState == PlayStateEnum.Reverse && chkLoopAnimation.Checked)
			{
				mAnimationFrameIndex = mAnimationFrameCount - 1;
				mAnimationKeyframeIndex = mAnimationKeyframeCount - 1;
				UpdateKeyframe();
			}
			//else if(mPlayState == PlayStateEnum.Play)
			//{
			//	mPlayState = PlayStateEnum.Stop;
			//	UpdatePlayState();
			//}
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IncrementFrameIndex																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Increment the animation frame index.
		/// </summary>
		private void IncrementFrameIndex()
		{
			if(mAnimationFrameIndex < mAnimationFrameCount - 1)
			{
				mAnimationFrameIndex++;
			}
			else
			{
				IncrementKeyframeIndex();
			}
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IncrementKeyframeIndex																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Increment the current animation keyframe index.
		/// </summary>
		private void IncrementKeyframeIndex()
		{
			if(mAnimationKeyframeIndex < mAnimationKeyframeCount - 1)
			{
				mAnimationFrameIndex = 0;
				mAnimationKeyframeIndex++;
				UpdateKeyframe();
			}
			else if(chkLoopAnimation.Checked)
			{
				mAnimationFrameIndex = 0;
				mAnimationKeyframeIndex = 0;
				UpdateKeyframe();
			}
			else if(mPlayState == PlayStateEnum.Play)
			{
				mPlayState = PlayStateEnum.Stop;
				UpdatePlayState();
				Status("Animation stopped.");
			}
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mTimerState_Tick																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// State timer.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mTimerState_Tick(object sender, EventArgs e)
		{
			if(mTurntable)
			{
				mOrbitIndex++;
				if(mOrbitIndex >= mOrbitPathCount)
				{
					mOrbitIndex = 0;
				}
				UpdateOrbit();
			}
			switch(mPlayState)
			{
				case PlayStateEnum.Pause:
					break;
				case PlayStateEnum.Play:
					IncrementFrameIndex();
					break;
				case PlayStateEnum.Reverse:
					break;
				case PlayStateEnum.Stop:
				default:
					break;
			}
			mAnimationTimeIndex++;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OrbitCamera																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Orbit the camera around the centerpiece.
		/// </summary>
		/// <param name="pitchDelta">
		/// The change in pitch.
		/// </param>
		/// <param name="yawDelta">
		/// The change in yaw.
		/// </param>
		private void OrbitCamera(float pitchDelta, float yawDelta)
		{
			mPitch += pitchDelta;
			mYaw += yawDelta;

			// Constrain pitch if needed (e.g., -90 to 90 degrees)
			mPitch = Math.Clamp(mPitch, -89.9f, 89.9f);

			//UpdateViewMatrix();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_MouseDown																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse button has been pressed on the preview panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlPreview_MouseDown(object sender, MouseEventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_MouseMove																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The mouse is moving over the preview panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlPreview_MouseMove(object sender, MouseEventArgs e)
		{
			int count = 0;
			int distance = 0;
			int index = 0;

			if(mAnimationTimeIndex > 10 ||
				e.Button != MouseButtons.None)
			{
				chkTurntable.Checked = false;
			}
			index = mOrbitIndex;
			switch(e.Button)
			{
				case MouseButtons.Left:
					//	Horizontal.
					if(mMouseLastX != 0)
					{
						distance = 0 - Clamp(e.X - mMouseLastX, -5, 5);
					}
					// Trace.WriteLine($"pnlPreview. x distance: {distance}");
					if(distance != 0)
					{
						index = mOrbitIndex + distance;
						count = mOrbitPathCount;
						if(distance > 0)
						{
							//	Mouse moved right. Orbit right.
							while(index > count - 1)
							{
								index -= count;
							}
						}
						else
						{
							//	Move moved left. Orbit left.
							while(index < 0)
							{
								index += count;
							}
						}
						mOrbitIndex = index;
					}
					//	Vertical.
					if(mMouseLastY != 0)
					{
						distance = Clamp(e.Y - mMouseLastY, -5, 5);
					}
					//Trace.WriteLine($"pnlPreview. y distance: {distance}");
					if(distance != 0)
					{
						index = mElevationIndex + distance;
						count = mElevationPathCount;
						if(distance > 0)
						{
							//	Mouse moved down. Orbit up.
							while(index < 0)
							{
								index += count;
							}
							//	Elevation doesn't wrap.
							if(index > count - 1)
							{
								index = count - 1;
							}
						}
						else
						{
							//	Mouse moved up. Orbit down.
							while(index > count - 1)
							{
								index -= count;
							}
							//	Elevation doesn't wrap.
							if(index < 0)
							{
								index = 0;
							}
						}
						mElevationIndex = index;
					}
					UpdateOrbit();
					break;
				case MouseButtons.Right:
					break;
				case MouseButtons.None:
				default:
					break;
			}
			mMouseLastX = e.X;
			mMouseLastY = e.Y;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_MouseWheel																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The scroll wheel has been turned.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Mouse event arguments.
		/// </param>
		private void pnlPreview_MouseWheel(object sender, MouseEventArgs e)
		{
			if(e.Delta > 0)
			{
				udZMag.UpButton();
			}
			else
			{
				udZMag.DownButton();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_Paint																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The preview panel is being painted.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		private void pnlPreview_Paint(object sender, PaintEventArgs e)
		{
			//	Do magic here.
			Graphics graphics = e.Graphics;
			int index = 0;
			FLine line = null;
			Brush materialBrush =
				new SolidBrush(ColorTranslator.FromHtml("#7fc0980e"));
			Pen materialPen = new Pen(ColorTranslator.FromHtml("#7f000000"));
			float[] moveDashes = { 8f, 4f };
			Pen movePen = new Pen(ColorTranslator.FromHtml("#ff9d9d9d"), 1f)
			{
				DashPattern = moveDashes
			};
			Pen plotPen = new Pen(ColorTranslator.FromHtml("#ff02007f"), 1f);
			Pen plungePen = new Pen(ColorTranslator.FromHtml("#ffff0000"), 1f);
			FVector2 point = null;
			Point[] pointArray = null;
			List<Point> pointList = new List<Point>();
			float progress = 0f;
			Brush surfaceBrush =
				new SolidBrush(ColorTranslator.FromHtml("#faecb7"));
			Image tool = ResourceMain.ToolPin2445;
			FVector3 toolLocation = new FVector3();
			Image upText = ResourceMain.UpTextIcon;
			Pen xPen = new Pen(ColorTranslator.FromHtml("#7f0000"));
			Pen yPen = new Pen(ColorTranslator.FromHtml("#007f00"));

			graphics.CompositingMode = CompositingMode.SourceOver;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.Bicubic;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			//	Draw the table.
			foreach(FVector3 pointItem in mTablePolygon)
			{
				point = mCamera.ProjectToScreen(pointItem);
				pointList.Add(ToPoint(point));
			}
			pointArray = pointList.ToArray();
			graphics.FillPolygon(surfaceBrush, pointArray);

			//	Draw the grid lines.
			index = 0;
			foreach(FLine3 lineItem in mTableGridX)
			{
				//	NORM: Project the line with no trace.
				line = mCamera.ProjectToScreen(lineItem);
				////	TEXT: Trace first line.
				//line = mCamera.ProjectToScreen(lineItem, index == 0);
				graphics.DrawLine(xPen, ToPoint(line.PointA), ToPoint(line.PointB));
				index++;
			}
			foreach(FLine3 lineItem in mTableGridY)
			{
				line = mCamera.ProjectToScreen(lineItem);
				graphics.DrawLine(yPen, ToPoint(line.PointA), ToPoint(line.PointB));
			}

			//	Draw the material.
			pointList.Clear();
			foreach(FVector3 pointItem in mMaterialPolygon)
			{
				point = mCamera.ProjectToScreen(pointItem);
				pointList.Add(ToPoint(point));
			}
			pointArray = pointList.ToArray();
			graphics.FillPolygon(materialBrush, pointArray);
			graphics.DrawPolygon(materialPen, pointArray);

			//	Draw the Up text.
			point = mCamera.ProjectToScreen(
				new FVector3(mWorkArea[0, 3],
				mWorkArea[1, 1] - (mWorkArea[0, 1] * 0.1f), 0f));
			graphics.DrawImage(upText,
				(int)point.X - (upText.Width / 2),
				(int)point.Y - (upText.Height / 2),
				upText.Width, upText.Height);


			//	Draw the tool position.
			if(mDataBusy || mAnimationKeyframeSegment == null)
			{
				//	No active keyframe.
				if(mTrackLayers.Count > 0 && mTrackLayers[0].Segments.Count > 0)
				{
					//	A first segment exists. Place the tool at the beginning of
					//	that segment.
					toolLocation =
						FVector3.Clone(mTrackLayers[0].Segments[0].StartOffset);
				}
				//	Where there is no first segment, the tool is placed at 0,0,0.
			}
			else
			{
				//	Active keyframe is known.
				if(mAnimationFrameIndex > 0)
				{
					//	Determine the intermediate position between the beginning and
					//	the end of the line.
					if(mAnimationFrameCount > 1)
					{
						progress = (float)mAnimationFrameIndex /
							((float)mAnimationFrameCount - 1f);
					}
					else if(mAnimationFrameCount == 1)
					{
						progress = (mAnimationFrameIndex == 0 ? 0f : 1f);
					}
					toolLocation = Linear.Lerp(
						mAnimationKeyframeSegment.StartOffset,
						mAnimationKeyframeSegment.EndOffset, progress);
				}
				else
				{
					//	When animation frame is zero, the tool is positioned at the
					//	beginning of the line.
					toolLocation = FVector3.Clone(mAnimationKeyframeSegment.StartOffset);
				}
			}

			if(!mDataBusy)
			{
				//	Draw the cut lines.
				//	In this version, the TrackViewLayerCollection contains every line
				//	to be drawn in world coordinates. Each of these only needs to be
				//	projected to the screen coordinates during refresh or update.
				index = 0;
				foreach(TrackViewLayerItem layerItem in mTrackLayers)
				{
					//	Each layer.
					foreach(TrackViewSegmentItem segmentItem in layerItem.Segments)
					{
						if(segmentItem.Visible)
						{
							line = mCamera.ProjectToScreen(segmentItem.Line);
							switch(segmentItem.SegmentType)
							{
								case TrackSegmentType.Plot:
									graphics.DrawLine(plotPen,
										ToPoint(line.PointA), ToPoint(line.PointB));
									break;
								case TrackSegmentType.Plunge:
									graphics.DrawLine(plungePen,
										ToPoint(line.PointA), ToPoint(line.PointB));
									break;
								case TrackSegmentType.Transit:
								default:
									graphics.DrawLine(movePen,
										ToPoint(line.PointA), ToPoint(line.PointB));
									break;
							}
							index++;
						}
					}
				}
			}
			toolLocation = mCamera.ProjectToScreen(toolLocation);
			graphics.DrawImage(tool,
				(int)toolLocation.X - (tool.Width / 4),
				(int)toolLocation.Y - (tool.Height / 2),
				tool.Width / 2, tool.Height / 2);

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_Resize																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The preview panel has been resized.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void pnlPreview_Resize(object sender, EventArgs e)
		{
			mCamera.DisplayWidth = pnlPreview.Width;
			mCamera.DisplayHeight = pnlPreview.Height;
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Status																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Display a status message.
		/// </summary>
		/// <param name="message">
		/// The message to display.
		/// </param>
		/// <param name="important">
		/// Value indicating whether to treat the message as important.
		/// </param>
		private void Status(string message, bool important = false)
		{
			if(message?.Length > 0)
			{
				lblStat.Text = message;
				lblStat.ForeColor = (important ? Color.DarkRed : Color.Black);
			}
			else
			{
				lblStat.Text = "";
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* udZMag_ValueChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Z magnification value has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void udZMag_ValueChanged(object sender, EventArgs e)
		{
			if(!mDataBusy)
			{
				mDataBusy = true;
				mTrackLayers =
					TrackViewLayerCollection.Convert(mTracks, (int)udZMag.Value);
				mDataBusy = false;
			}
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateKeyframe																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the working parts for the current keyframe.
		/// </summary>
		private void UpdateKeyframe()
		{
			mAnimationKeyframeSegment =
				mTrackLayers.GetSegment(mAnimationKeyframeIndex);
			if(mAnimationKeyframeSegment != null)
			{
				mAnimationKeyframeSegment.Visible = true;
				if(chkHideTracksUntilVisited.Checked)
				{
					mTrackLayers.ShowSegmentsBefore(mAnimationKeyframeSegment);
					mTrackLayers.HideSegmentsAfter(mAnimationKeyframeSegment);
				}
				lblToolStat.Text =
					(mAnimationKeyframeSegment?.ParentLayer?.ToolName?.Length > 0 ?
						mAnimationKeyframeSegment?.ParentLayer?.ToolName :
						"No tool selected...");
				if(mMultipleToolsDeclared)
				{
					if(mAnimationKeyframeLayer?.ToolName !=
						mAnimationKeyframeSegment.ParentLayer?.ToolName)
					{
						if((mPlayState == PlayStateEnum.Play ||
							mPlayState == PlayStateEnum.Pause) &&
							mAnimationKeyframeSegment.ParentLayer != null)
						{
							//	If a tool has already been set, then add the change tool
							//	message and pause.
							Status(
								"Please insert tool: " +
								mAnimationKeyframeSegment.ParentLayer.ToolName, true);
							mPlayState = PlayStateEnum.Pause;
							UpdatePlayState();
						}
					}
				}
				if(mAnimationKeyframeSegment.ParentLayer != null)
				{
					mAnimationKeyframeLayer = mAnimationKeyframeSegment.ParentLayer;
				}
			}
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateOrbit																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the orbit position of the camera.
		/// </summary>
		private void UpdateOrbit()
		{
			FVector3 camPosition = null;
			FVector3 center = null;
			double elevationAngle = 0d;
			double orbitAngle = 0d;
			//FVector2 point = null;

			if(mOrbitIndex < mOrbitPathCount &&
				mElevationIndex < mElevationPathCount)
			{
				center = new FVector3(0f, mOrbitRadius, 0f);
				orbitAngle = ((double)mOrbitIndex / (double)mOrbitPathCount) *
					(double)GeometryUtil.TwoPi;
				elevationAngle = 0.01745329251994329576923690768489 +
					(((double)mElevationIndex / (double)mElevationPathCount) *
					1.3962634015954636615389526147909d);

				camPosition = new FVector3(FMatrix3.Rotate(center,
					(float)elevationAngle, 0f, (float)orbitAngle));
				camPosition += mCamera.LookAt;

				mCamera.Position = camPosition;
				pnlPreview.Invalidate();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdatePlayState																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the states of the buttons according to the current play mode.
		/// </summary>
		private void UpdatePlayState()
		{
			switch(mPlayState)
			{
				case PlayStateEnum.Pause:
					btnPause.Enabled = true;
					btnPlay.Enabled = true;
					btnStop.Enabled = true;
					break;
				case PlayStateEnum.Play:
					btnPause.Enabled = true;
					btnPlay.Enabled = false;
					btnStop.Enabled = true;
					break;
				case PlayStateEnum.Reverse:
					break;
				case PlayStateEnum.Stop:
				default:
					mAnimationKeyframeIndex = 0;
					mAnimationKeyframeSegment = null;
					mAnimationFrameIndex = 0;
					btnPause.Enabled = false;
					btnPlay.Enabled = true;
					btnStop.Enabled = false;
					break;
			}
			btnFirst.Enabled =
				btnPrevious.Enabled =
					mAnimationKeyframeIndex > 0;
			btnLast.Enabled =
				btnNext.Enabled =
					(mAnimationKeyframeIndex + 1 < mAnimationKeyframeCount) ||
					(mAnimationFrameIndex + 1 < mAnimationFrameCount);
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnActivated																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Activated event when the form has been activated.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if(!mActivated)
			{
				mTracks = new TrackLayerCollection(SessionWorkpieceInfo.Cuts);
				mTrackLayers =
					TrackViewLayerCollection.Convert(mTracks, (int)udZMag.Value);
				mMultipleToolsDeclared = mTrackLayers.GetMultipleToolsDeclared();
				mAnimationKeyframeCount = 0;
				foreach(TrackViewLayerItem layerItem in mTrackLayers)
				{
					mAnimationKeyframeCount += layerItem.Segments.Count;
				}
				mActivated = true;
				Status("Ready...");
				UpdatePlayState();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Load event when the form has been loaded and is ready to
		/// display for the first time.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmView3D Item.
		/// </summary>
		public frmView3D()
		{
			FVector2 absPoint = null;
			FArea area = null;
			FVector3 camPosition = null;
			float col = 0f;
			float gridCount = 8;
			float row = 0f;
			float x = 0f;
			float y = 0f;

			InitializeComponent();

			//	Double-buffering on Preview panel.
			typeof(Panel).InvokeMember("DoubleBuffered",
				BindingFlags.SetProperty | BindingFlags.Instance |
				BindingFlags.NonPublic,
				null, pnlPreview, new object[] { true });

			this.DoubleBuffered = true;

			//	Configure the work area.
			mWorkArea[0, 0] = GetMillimeters(ConfigProfile.XDimension);
			mWorkArea[1, 0] = GetMillimeters(ConfigProfile.YDimension);

			//	Set X.
			switch(ConfigProfile.XYOrigin)
			{
				case OriginLocationEnum.Bottom:
				case OriginLocationEnum.Center:
				case OriginLocationEnum.Top:
					//	Bottom center.
					//	Middle center.
					//	Top center.
					mWorkArea[0, 3] = 0f;
					if(ConfigProfile.TravelX == DirectionLeftRightEnum.Left)
					{
						//	X increases left.
						mWorkArea[0, 1] = mWorkArea[0, 0] / 2f;
						mWorkArea[0, 2] = 0f - (mWorkArea[0, 0] / 2f);
					}
					else
					{
						//	X increases right.
						mWorkArea[0, 1] = 0f - (mWorkArea[0, 0] / 2f);
						mWorkArea[0, 2] = mWorkArea[0, 0] / 2f;
					}
					break;
				case OriginLocationEnum.BottomLeft:
				case OriginLocationEnum.Left:
				case OriginLocationEnum.TopLeft:
					//	Bottom left.
					//	Middle left.
					//	Top left.
					mWorkArea[0, 1] = 0f;
					if(ConfigProfile.TravelX == DirectionLeftRightEnum.Left)
					{
						//	X increases left.
						mWorkArea[0, 2] = 0f - mWorkArea[0, 0];
						mWorkArea[0, 3] = 0f - (mWorkArea[0, 0] / 2f);
					}
					else
					{
						//	X increases right.
						mWorkArea[0, 2] = mWorkArea[0, 0];
						mWorkArea[0, 3] = mWorkArea[0, 0] / 2f;
					}
					break;
				case OriginLocationEnum.BottomRight:
				case OriginLocationEnum.Right:
				case OriginLocationEnum.TopRight:
					//	Bottom right.
					//	Middle right.
					//	Top right.
					mWorkArea[0, 2] = 0f;
					if(ConfigProfile.TravelX == DirectionLeftRightEnum.Left)
					{
						//	X increases left.
						mWorkArea[0, 1] = mWorkArea[0, 0];
						mWorkArea[0, 3] = mWorkArea[0, 0] / 2f;
					}
					else
					{
						//	X increases right.
						mWorkArea[0, 1] = 0f - mWorkArea[0, 0];
						mWorkArea[0, 3] = 0f - (mWorkArea[0, 0] / 2f);
					}
					break;
			}
			//	Set Y.
			switch(ConfigProfile.XYOrigin)
			{
				case OriginLocationEnum.Bottom:
				case OriginLocationEnum.BottomLeft:
				case OriginLocationEnum.BottomRight:
					mWorkArea[1, 2] = 0f;
					if(ConfigProfile.TravelY == DirectionUpDownEnum.Down)
					{
						mWorkArea[1, 1] = 0f - mWorkArea[1, 0];
						mWorkArea[1, 3] = 0f - (mWorkArea[1, 0] / 2f);
					}
					else
					{
						mWorkArea[1, 1] = mWorkArea[1, 0];
						mWorkArea[1, 3] = mWorkArea[1, 0] / 2f;
					}
					break;
				case OriginLocationEnum.Center:
				case OriginLocationEnum.Left:
				case OriginLocationEnum.Right:
					mWorkArea[1, 3] = 0f;
					if(ConfigProfile.TravelY == DirectionUpDownEnum.Down)
					{
						mWorkArea[1, 1] = 0f - (mWorkArea[1, 0] / 2f);
						mWorkArea[1, 2] = mWorkArea[1, 0] / 2f;
					}
					else
					{
						mWorkArea[1, 1] = mWorkArea[1, 0] / 2f;
						mWorkArea[1, 2] = 0f - (mWorkArea[1, 0] / 2f);
					}
					break;
				case OriginLocationEnum.Top:
				case OriginLocationEnum.TopLeft:
				case OriginLocationEnum.TopRight:
					mWorkArea[1, 1] = 0f;
					if(ConfigProfile.TravelY == DirectionUpDownEnum.Down)
					{
						mWorkArea[1, 2] = mWorkArea[1, 0];
						mWorkArea[1, 3] = mWorkArea[1, 0] / 2f;
					}
					else
					{
						mWorkArea[1, 2] = 0f - mWorkArea[1, 0];
						mWorkArea[1, 2] = 0f - (mWorkArea[1, 0] / 2f);
					}
					break;
			}
#if CameraPerspective
			mOrbitRadius = Math.Max(mWorkArea[0, 0], mWorkArea[1, 0]) * 1.25f;
#else
			//	NORM: Orbit horizontally around object.
			mOrbitRadius = Math.Max(mWorkArea[0, 0], mWorkArea[1, 0]) * 0.75f;
			////	TEST: Orbit vertically around object.
			//mOrbitRadius = Math.Max(mWorkArea[0, 0], mWorkArea[1, 0]);
#endif

			//	Create the table polygon.
			mTablePolygon.Add(new FVector3(mWorkArea[0, 1], mWorkArea[1, 1], 0f));
			mTablePolygon.Add(new FVector3(mWorkArea[0, 1], mWorkArea[1, 2], 0f));
			mTablePolygon.Add(new FVector3(mWorkArea[0, 2], mWorkArea[1, 2], 0f));
			mTablePolygon.Add(new FVector3(mWorkArea[0, 2], mWorkArea[1, 1], 0f));


			//	Create the material polygon.
			area = SessionWorkpieceInfo.Area;
			absPoint = TransformToAbsolute(new FVector2(area.Left, area.Top));
			mMaterialPolygon.Add(new FVector3(absPoint.X, absPoint.Y, 0f));
			absPoint = TransformToAbsolute(new FVector2(area.Left, area.Bottom));
			mMaterialPolygon.Add(new FVector3(absPoint.X, absPoint.Y, 0f));
			absPoint = TransformToAbsolute(new FVector2(area.Right, area.Bottom));
			mMaterialPolygon.Add(new FVector3(absPoint.X, absPoint.Y, 0f));
			absPoint = TransformToAbsolute(new FVector2(area.Right, area.Top));
			mMaterialPolygon.Add(new FVector3(absPoint.X, absPoint.Y, 0f));

			//	Create the X grid.
			for(row = 0f; row <= gridCount; row ++)
			{
				y = mWorkArea[1, 1] +
					(row * ((mWorkArea[1, 2] - mWorkArea[1, 1]) / gridCount));
				mTableGridX.Add(new FLine3(
					new FVector3(mWorkArea[0, 1], y, 0f),
					new FVector3(mWorkArea[0, 2], y, 0f),
					new FColor4(0.8f, 0.5f, 0f, 0f)
					));
			}
			//	Create the Y grid.
			for(col = 0f; col <= gridCount; col++)
			{
				x = mWorkArea[0, 1] +
					(col * ((mWorkArea[0, 2] - mWorkArea[0, 1]) / gridCount));
				mTableGridY.Add(new FLine3(
					new FVector3(x, mWorkArea[1, 1], 0f),
					new FVector3(x, mWorkArea[1, 2], 0f),
					new FColor4(0.8f, 0f, 0.5f, 0f)
					));
			}

			//	Initialize the camera.
			mOrbitIndex = mOrbitPathCount / 2;
			//point = mOrbitPath[mOrbitIndex];
			mCamera.DisplayWidth = pnlPreview.Width;
			mCamera.DisplayHeight = pnlPreview.Height;
#if CameraPerspective
			//	Perspective camera only.
			mCamera.FieldOfView = 90f;
#else
			// Orthographic camera only.
			mCamera.TargetObjectWidth =
				Math.Max(GetMillimeters(ConfigProfile.XDimension),
					GetMillimeters(ConfigProfile.YDimension)) + 100f;
#endif
			mCamera.LookAt = new FVector3(mWorkArea[0, 3], mWorkArea[1, 3], 0f);
			////	NORM: Orbit horizontally around object.
			//camPosition = new FVector3(
			//	point.X, point.Y, mOrbitRadius / 4f);
			////	TEST: Orbit vertically around object.
			//camPosition = new FVector3(
			//	0f, point.Y, point.X);
			//Trace.WriteLine(
			//	$"Pos: '{mOrbitIndex.ToString().PadLeft(2, '0')}', " +
			//	$"Camera: {{{camPosition}}},");
			mCamera.Position = camPosition;


			pnlPreview.MouseMove += pnlPreview_MouseMove;
			pnlPreview.MouseDown += pnlPreview_MouseDown;
			pnlPreview.MouseWheel += pnlPreview_MouseWheel;
			pnlPreview.Paint += pnlPreview_Paint;
			pnlPreview.Resize += pnlPreview_Resize;

			//	Buttons.
			this.CancelButton = btnCancel;
			//this.AcceptButton = btnOK;
			btnCancel.Click += btnCancel_Click;
			btnFirst.Click += btnFirst_Click;
			btnLast.Click += btnLast_Click;
			btnNext.Click += btnNext_Click;
			btnOK.Click += btnOK_Click;
			btnPause.Click += btnPause_Click;
			btnPlay.Click += btnPlay_Click;
			btnPrevious.Click += btnPrevious_Click;
			btnStop.Click += btnStop_Click;

			//	Checkboxes.
			chkHideTracksUntilVisited.CheckedChanged +=
				chkHideTracksUntilVisited_CheckedChanged;
			chkTurntable.CheckedChanged += chkTurntable_CheckedChanged;

			//	Up/Down.
			udZMag.ValueChanged += udZMag_ValueChanged;

			mPlayState = PlayStateEnum.Stop;
			UpdatePlayState();

			//	Timers.
			mTimerState.Tick += mTimerState_Tick;
			mTimerState.Start();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WorkArea																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="WorkArea">WorkArea</see>.
		/// </summary>
		private float[,] mWorkArea = new float[2, 4];
		/// <summary>
		/// Get a reference to the work area for this preview.
		/// </summary>
		/// <remarks>
		/// The contents of this value are:
		/// Row (0): 0-X, 1-Y.
		/// Col (1): 0-Width/Height, 1-Left/Top, 2-Right/Bottom, 3-Center/Middle.
		/// </remarks>
		public float[,] WorkArea
		{
			get { return mWorkArea; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
