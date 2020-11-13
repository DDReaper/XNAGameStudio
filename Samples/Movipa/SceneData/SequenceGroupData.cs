#region File Description
//-----------------------------------------------------------------------------
// SequenceGroupData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SceneDataLibrary
{
    /// <summary>
    /// This class manages the list of pattern groups displayed in a sequence.
    /// In Layout, sequence groups correspond to these pattern groups.
    /// The Update function sets the time forward and calculates coordinates 
    /// or color values,  and the calculation results are temporarily stored 
    /// in pattern objects.
    /// These results can be used to display other display items by 
    /// synchronizing them with sequence animations.
    /// However, attention is required when creating scenes by Layout, because 
    /// it is not assumed that the same pattern object data is referred to
    /// in the same game scene.
    ///
    /// �V�[�P���X�ŕ\������p�^�[���O���[�v�̃��X�g��ێ����܂��B
    /// Layout�ł̓V�[�P���X�O���[�v�ɑ������܂��B
    /// Update�֐��ŁA������i�߁A���W��F���̍Čv�Z���s���܂��B
    /// �v�Z���ʂ̓p�^�[���I�u�W�F�N�g�f�[�^�Ɉꎞ�ۑ�����܂��̂ŁA
    /// ���̌��ʂ�p���āA���̕\�������V�[�P���X�A�j���[�V������
    /// ���������ĕ\�������邱�Ƃ��\�ł��B
    /// �������A�����Q�[���V�[�����œ����p�^�[���I�u�W�F�N�g�f�[�^��
    /// �Q�Ƃ����󋵂͑z�肵�Ă��Ȃ����߁A
    /// Layout�ŃV�[���쐬����ۂ͒��ӂ��K�v�ł��B
    /// </summary>
    public class SequenceGroupData
    {
        #region Public Types
        //Animation interpolation type
        //
        //�A�j���[�V������ԃ^�C�v
        public enum Interpolation
        {
            None = 0,//No interpolation //��Ԗ���
            Linear,//Linear interpolation //���`���
            Spline,//Spline interpolation //�X�v���C�����
        }

        public enum PlayStatus
        {
            Playing = 0,//Playing //�Đ���
            Stop,//Stopped //��~��
            LoopEnd,//In loop //���[�v��
        }
        #endregion

        #region Fields
        private int startFrame = 0;//Start frame for animation movement 
        private int loopNumber = 0;//Maximum loop count

        private TimeSpan timeFrame = new TimeSpan(0, 0, 0);//Current frame
        private PlayStatus playStatus = PlayStatus.Stop;//Play status 
        private long framePerSecond = 60;//The number of frames per second
        private int loopCount = 0;//Current loop count 
        private int drawObjectId = -1;//Base object (Object to be drawn)

        private Interpolation interpolationType = Interpolation.None;
        private int splineParamT = 0;//Spline curve parameter T
        private int splineParamC = 0;//Spline curve parameter C
        private int splineParamB = 0;//Spline curve parameter B

        private float[] tcbParam = new float[4];//Spline calculation parameter 

        //List of sequence objects 
        private List<SequenceObjectData> objectList = new List<SequenceObjectData>();
        #endregion

        #region Property
        /// <summary>
        /// Obtains and sets the start frame for animation movement.
        ///
        /// �A�j���[�V��������̊J�n�t���[����ݒ�擾���܂��B
        /// </summary>
        public int StartFrame
        {
            get
            {
                return startFrame;
            }
            set
            {
                startFrame = value;
            }
        }

        /// <summary>
        /// Obtains and sets the maximum loop count.
        ///
        /// ���[�v����ݒ�擾���܂��B
        /// </summary>
        public int LoopNumber
        {
            get
            {
                return loopNumber;
            }
            set
            {
                loopNumber = value;
            }
        }

        /// <summary>
        /// Obtains and sets the animation interpolation type.
        /// </summary>
        public Interpolation InterpolationType
        {
            get
            {
                return interpolationType;
            }
            set
            {
                interpolationType = value;
            }
        }

        /// <summary>
        /// Obtains and sets the spline curve parameter T.
        ///
        /// �X�v���C����ԗp�̃p�����[�^��ݒ�擾���܂��B
        /// </summary>
        public int SplineParamT
        {
            get
            {
                return splineParamT;
            }
            set
            {
                splineParamT = value;
            }
        }

        /// <summary>
        /// Obtains and sets the spline curve parameter C.
        ///
        /// �X�v���C����ԗp�̃p�����[�^��ݒ�擾���܂��B
        /// </summary>
        public int SplineParamC
        {
            get
            {
                return splineParamC;
            }
            set
            {
                splineParamC = value;
            }
        }

        /// <summary>
        /// Obtains and sets the spline curve parameter B.
        ///
        /// �X�v���C����ԗp�̃p�����[�^��ݒ�擾���܂��B
        /// </summary>
        public int SplineParamB
        {
            get
            {
                return splineParamB;
            }
            set
            {
                splineParamB = value;
            }
        }

        /// <summary>
        /// Obtains the list of sequence objects.
        ///
        /// �X�v���C����ԗp�̃p�����[�^��ݒ�擾���܂��B
        /// </summary>
        public List<SequenceObjectData> SequenceObjectList
        {
            get { return objectList; }
        }

        /// <summary>
        /// If the sequence is stopped, returns true.
        ///
        /// �V�[�P���X����~���Ȃ�true
        /// </summary>
        public bool IsStop
        {
            get
            {
                return (PlayStatus.Stop == playStatus);
            }
        }

        /// <summary>
        /// If the sequence is in a loop, returns true.
        ///
        /// �V�[�P���X�����[�v���Ȃ�true
        /// </summary>
        public bool IsLoopEnd
        {
            get
            {
                return (PlayStatus.LoopEnd == playStatus);
            }
        }
        #endregion

        
        /// <summary>
        /// Performs initialization.
        /// Converts TCB curve information to runtime format.
        ///
        /// ���������܂��B
        /// TCB�Ȑ��̏������s���̌`���ɕϊ����܂��B
        /// </summary>
        public void Init()
        {
            //Calculates the parameters for spline interpolation.
            //
            //�X�v���C����ԗp�̃p�����[�^�̌v�Z
            tcbParam[0] = (1.0f - SplineParamT) * (1.0f + SplineParamC) * 
                (1.0f + SplineParamB);
            tcbParam[1] = (1.0f - SplineParamT) * (1.0f - SplineParamC) * 
                (1.0f - SplineParamB);
            tcbParam[2] = (1.0f - SplineParamT) * (1.0f - SplineParamC) * 
                (1.0f + SplineParamB);
            tcbParam[3] = (1.0f - SplineParamT) * (1.0f + SplineParamC) * 
                (1.0f - SplineParamB);
        }


        /// <summary>
        /// Sets the animation time forward.
        /// Updates the current time by adding the difference between
        /// the current time and the forwarded time.
        /// If the updated time exceeds the play time of the entire animation, 
        /// consider whether to perform loop processes and determine the frames 
        /// corrected if necessary.
        ///
        /// �A�j���[�V�����̎��Ԃ�i�߂܂��B
        /// ���݂̎����ɂ����߂鎞�Ԃ����Z���āA���݂̎������X�V���܂��B
        /// �A�j���[�V�����S�̂̒�����茻�݂̎������������ꍇ��
        /// ���[�v�̗L���Ȃǂ��������A�K�v�ł���Ε␳�����t���[��������o���܂��B
        /// </summary>
        /// <param name="fPlayFrames">
        /// Current time
        /// 
        /// ���݂̎���
        /// </param>
        /// <param name="ElapsedGameTime">
        /// Time to be forwarded
        /// 
        /// �i�߂鎞��
        /// </param>
        /// <param name="bReverse">
        /// Specifies true in case of reverse play
        /// 
        /// �t�Đ��̏ꍇtrue
        /// </param>
        /// <returns>
        /// Returns the time specified in the sequence object to be displayed
        /// 
        /// �\������V�[�P���X�I�u�W�F�N�g�ɐݒ肳�ꂽ���Ԃ�Ԃ��܂�
        /// </returns>
        public float Update(float playFrames, TimeSpan elapsedGameTime, bool reverse)
        {
            //Updates the time.
            //
            //���Ԃ̍X�V
            if (reverse)
                timeFrame += elapsedGameTime;
            else
                timeFrame += elapsedGameTime;

            if (TimeSpan.Zero > timeFrame)
                timeFrame = TimeSpan.Zero;

            //Clears the play status.
            //
            //�Đ���Ԃ̃N���A
            playStatus = PlayStatus.Playing;

        BEGIN_UPDATE:

            //Calculates the length of the entire sequence.
            //
            //�V�[�P���X�S�̂̒������v�Z���܂��B
            int total = StartFrame;
            int length = StartFrame;
            foreach (SequenceObjectData sequenceObject in objectList)
            {
                length += sequenceObject.Frame;
            }

            //Checks whether the process proceeds to the end of the sequence.
            //
            //�V�[�P���X�̖��[�܂Ői�񂾂��ǂ����m�F���܂��B
            if (timeFrame.Ticks * framePerSecond / TimeSpan.TicksPerSecond >= length)
            {
                //When the process proceeds to the end of the sequence.
                //
                //���[�܂Ői��ł���ꍇ�B
                playStatus = PlayStatus.LoopEnd;

                //If the maximum loop count is less than 0, 
                // there is no limitation on the loop count.
                //
                //���[�v�K�萔�����̏ꍇ�A�����Ƀ��[�v���܂��B
                if (0 > LoopNumber)
                {
                    //Sets the status of after-loop.
                    //
                    //���[�v������̏�Ԃɂ��Đݒ肵�܂��B
                    if (1 < objectList.Count)
                    {
                        timeFrame = new TimeSpan(
                            (
                            (timeFrame.Ticks * framePerSecond / TimeSpan.TicksPerSecond)
                            % length
                            )
                            * TimeSpan.TicksPerSecond / framePerSecond);
                        
                        //Performs recalculation based on the corrected frame.
                        //
                        //�␳���ꂽ�t���[�������ɍČv�Z����
                        goto BEGIN_UPDATE;
                    }
                    else
                    {
                        drawObjectId = 0;
                        playStatus = PlayStatus.Stop;
                        playFrames = 0.0f;
                    }
                }
                else
                {
                    //Updates the current loop count. //���[�v�����񐔂��X�V���܂��B
                    loopCount += (int)(
                        (timeFrame.Ticks * framePerSecond / TimeSpan.TicksPerSecond)
                        / length);

                    //Updates the time.
                    //
                    //���Ԃ̍X�V
                    timeFrame -= new TimeSpan(
                        (
                        (timeFrame.Ticks * framePerSecond / TimeSpan.TicksPerSecond)
                        % length) 
                        * TimeSpan.TicksPerSecond / framePerSecond);

                    if (loopCount < LoopNumber)
                    {
                        //Performs recalculation based on the corrected frame.
                        //
                        //�␳���ꂽ�t���[�������ɍČv�Z����
                        goto BEGIN_UPDATE;
                    }

                    playStatus = PlayStatus.Stop;
                }
            }
            else if (reverse && TimeSpan.Zero == timeFrame)
            {
                //In reverse play, stops playing when the frame becomes 0.
                //
                //�t�Đ��ŁA�t���[�����O�ɂȂ����ꍇ��~���܂��B
                drawObjectId = 0;
                playStatus = PlayStatus.Stop;
                playFrames = 0.0f;
            }
            else if (timeFrame.Ticks * framePerSecond / TimeSpan.TicksPerSecond 
                >= StartFrame)
            {
                //When the process does not proceed to the end of animation, and
                //the frame is not yet 0 in reverse play.
                //(In other words, when in normal interpolation display operation.)
                //Other than these conditions,
                //
                //�A�j���[�V�������[�܂Ői��ł��Ȃ��ꍇ����
                //�t�Đ��Ńt���[���O�Ɏ����Ă��Ȃ��ꍇ�B
                //�܂�A�ʏ�̕�ԕ\���̏ꍇ�B
                //����ȊO�̏ꍇ�ł��A
                drawObjectId = -1;

                int i = 0;

                //searches sequence objects to be displayed.
                //
                //�\���Ώۂɂ���V�[�P���X�I�u�W�F�N�g���������܂��B
                foreach (SequenceObjectData data in objectList)
                {
                    total += data.Frame;

                    if (timeFrame.Ticks * framePerSecond / TimeSpan.TicksPerSecond 
                        < total)
                    {
                        //The object to be displayed is found.
                        //
                        //�\������I�u�W�F�N�g�𔭌�
                        drawObjectId = i;
                        break;
                    }

                    i++;
                }

                //When the object to be displayed is found.
                //
                //�\������I�u�W�F�N�g�����������ꍇ
                if (0 <= drawObjectId)
                {
                    //Calculates the time in the sequence object to be displayed.
                    //
                    //�\������V�[�P���X�I�u�W�F�N�g���ł̎��Ԃ��v�Z���܂��B
                    if (0 == objectList[drawObjectId].Frame)
                    {
                        playFrames = 0.0f;
                    }
                    else
                    {
                        playFrames = (
                            (float)timeFrame.Ticks * (float)framePerSecond /
                            (float)TimeSpan.TicksPerSecond - 
                            (float)(total - objectList[drawObjectId].Frame)) 
                            / (float)objectList[drawObjectId].Frame;
                    }
                }
            }
            else
            {
                //Displays nothing.
                //
                //�����\�����Ȃ�
                drawObjectId = -1;
            }

            // Once the frame has been set up, performs interpolation 
            // for conversion information.
            //
            //�t���[�����m�肵���̂ŁA�ϊ����̕�ԏ������s���܂��B
            updateSeq(playFrames);

            return playFrames;
        }


        /// <summary>
        /// Performs interpolation based on the provided display frame.
        /// Begins by determining the sequence objects to be displayed. 
        /// Then, if the interpolation type is Linear or Spline, performs
        /// conversion interpolation by using the information of neighboring 
        /// sequence objects as needed.
        ///
        /// �^����ꂽ�\���t���[�������ɕ�ԏ������s���܂��B
        /// �܂��A�\�����ׂ��V�[�P���X�I�u�W�F�N�g�����߁A
        /// ��ԃ^�C�v�����`�A�X�v���C���̏ꍇ�́A
        /// �K�v�ɉ����ċߗׂ̃V�[�P���X�I�u�W�F�N�g�̏���p����
        /// �ϊ��̕�Ԃ��s���܂��B
        /// </summary>
        /// <param name="fPlayFrames">
        /// Time in the current sequence object to be displayed
        /// 
        /// ���ݕ\�����ׂ��V�[�P���X�I�u�W�F�N�g���ł̎���
        /// </param>
        private void updateSeq(float playFrame)
        {
            //The following objects are needed for interpolation (at a maximum):
            //- Base object ...baseObject
            //- Previous object (Object before the Base object) ...prevObject
            //- Target object (Object after the Base object) ...targetObject
            //- Next object (Object after the Target object) ...nextObject
            //
            //��ԂɕK�v�Ȃ̂́A�ő��
            //�E���ڃI�u�W�F�N�g...baseObject
            //�E���ڃI�u�W�F�N�g�̑O�̃I�u�W�F�N�g...prevObject
            //�E���ڃI�u�W�F�N�g�̎��̃I�u�W�F�N�g...targetObject
            //�E���ڃI�u�W�F�N�g�̎��̎��̃I�u�W�F�N�g...nextObject
            //�ɂȂ�܂��B
            SequenceObjectData prevObject, baseObject, targetObject, nextObject;

            //If there is no object to be displayed, returns.
            //
            //�\������I�u�W�F�N�g�������Ȃ�߂�܂��B
            if (0 > drawObjectId)
                return;

            //Sets the objects used for interpolation.
            //
            //��Ԃ̂��߂Ɏg�p���邷��I�u�W�F�N�g��ݒ肵�܂��B

            //Clears the objects used for interpolation.
            //
            //��ԗp�̃I�u�W�F�N�g�̃N���A
            nextObject = targetObject = prevObject = baseObject
                                            = objectList[drawObjectId];
            //Obtains the Previous object.
            //
            //���O�̃I�u�W�F�N�g�̎擾
            if (0 < drawObjectId)
            {
                prevObject = objectList[drawObjectId - 1];
            }
            else if (0 != loopCount)
            {
                //Otherwise, uses the last object when performing the loop process.
                //
                //�����łȂ��ꍇ�A���[�v����Ȃ�Ō�̃I�u�W�F�N�g��p����
                prevObject = objectList[objectList.Count - 1];
            }

            //Obtains the Target object (and sets the Next object in advance).
            //
            //���̃I�u�W�F�N�g�̎擾("���̎�"�����炩���ߐݒ�)
            if (drawObjectId < objectList.Count - 1)
            {
                //When the Base object is not the last object.
                //
                //���ڃI�u�W�F�N�g�����Ō�̃I�u�W�F�N�g�ł͂Ȃ��B
                nextObject = targetObject = objectList[drawObjectId + 1];
            }
            else
            {
                //Sets the first object when performing the loop process.
                //
                //���[�v����Ȃ�A�ŏ��̃I�u�W�F�N�g�ɐݒ�
                if (loopCount < LoopNumber)
                {
                    nextObject = targetObject = objectList[0];
                }
            }

            //Obtains the Next object.
            //
            //���̎��̃I�u�W�F�N�g�̎擾
            if (drawObjectId < objectList.Count - 2)
            {
                //When the current index + 2 is valid.
                //
                //�Q���Index���L��
                nextObject = objectList[drawObjectId + 2];
            }
            else
            {
                //Loop process
                //
                //���[�v����
                if (loopCount < LoopNumber)
                {
                    //When the Next object is the first object.
                    //
                    //���̎��͍ŏ��̃I�u�W�F�N�g�B
                    if (drawObjectId == objectList.Count - 2)
                    {
                        nextObject = objectList[0];
                    }
                    else
                    {
                        //When there are 2 or more objects in total (Index 1 is valid).
                        //
                        //�S�̂̃I�u�W�F�N�g����2�ȏ�iIndex�P���L���j
                        if (1 < objectList.Count)
                            nextObject = objectList[1];
                        else
                            nextObject = objectList[0];
                    }
                }
            }

            //For each pattern object in the sequence object to be displayed, 
            //records the interpolated conversion information.
            //
            //�\������V�[�P���X�I�u�W�F�N�g���̃p�^�[���I�u�W�F�N�g���ꂼ��ɂ���
            //��Ԃ����ϊ������L�^���Ă����܂��B
            for (int i = 0; i < baseObject.PatternObjectList.Count; i++)
            {
                PatternObjectData prevPattern, basePattern;
                PatternObjectData targetPattern, nextPattern;

                //Sets the interpolation target pattern.
                //
                //��ԑΏۂ̃p�^�[����ݒ肵�܂��B
                prevPattern = basePattern = targetPattern = 
                    nextPattern = baseObject.PatternObjectList[i];

                if (targetObject.PatternObjectList.Count > i)
                    targetPattern = targetObject.PatternObjectList[i];

                if (prevObject.PatternObjectList.Count > i)
                    prevPattern = prevObject.PatternObjectList[i];

                if (nextObject.PatternObjectList.Count > i)
                    nextPattern = nextObject.PatternObjectList[i];

                //Calculates conversion values after interpolation.
                //
                //��Ԍ�̕ϊ������v�Z���܂��B
                DrawData data = new DrawData();
                switch (InterpolationType)
                {
                    case Interpolation.None:
                        data = basePattern.Data;
                        break;
                    case Interpolation.Linear:
                        PutInfoLinearInterporlation(playFrame, data, 
                            basePattern.Data, targetPattern.Data);
                        break;
                    case Interpolation.Spline:
                        PutInfoTCBSplineInterporlation(playFrame, data, 
                            prevPattern.Data, basePattern.Data, 
                            targetPattern.Data, nextPattern.Data);
                        break;
                }

                //Records the data to the pattern object.
                //The data recorded here can be used as position information for 
                //animated patterns.
                //
                //�p�^�[���I�u�W�F�N�g�ɋL�^���܂��B
                //�����ŋL�^���ꂽ���́A�A�j���[�V�������ꂽ�p�^�[����
                //�z�u���Ƃ��ė��p���邱�Ƃ��\�ł��B
                baseObject.PatternObjectList[i].InterpolationDrawData = data;

            }
        }

        /// <summary>
        /// Draws the objects by using the conversion information modified 
        /// by the Update function.  The conversion information can be also applied
        /// to the entire sequence (as offset).
        ///
        /// Update�֐��ōX�V���ꂽ�ϊ����𗘗p���Ȃ���`�悵�܂��B
        /// �V�[�P���X�S�̂ɕϊ����i�I�t�Z�b�g�Ƃ��āj�K�p���邱�Ƃ��o���܂��B
        /// </summary>
        /// <param name="sb">
        /// SpriteBatch
        /// 
        /// �X�v���C���o�b�`
        /// </param>
        /// <param name="fPlayFrames">
        /// Frame to be displayed
        /// 
        /// �\������t���[��
        /// </param>
        /// <param name="infoPut">
        /// Conversion information that affects the entire drawing target
        /// 
        /// �`��ΏۑS�̂ɉe������ϊ����
        /// </param>
        public void Draw(SpriteBatch sb, DrawData baseDrawData)
        {
            if (0 > drawObjectId)
            {
                return;
            }

            //Obtains the current Base object.
            //
            //���ݒ��ڂ��Ă���V�[�P���X�I�u�W�F�N�g���擾
            SequenceObjectData baseObj = objectList[drawObjectId];

            //Referred to by the sequence object.
            //
            //�V�[�P���X�I�u�W�F�N�g���Q�Ƃ���
            for (int i = 0; i < baseObj.PatternObjectList.Count; i++)
            {
                DrawData sqInfo =
                    baseObj.PatternObjectList[i].InterpolationDrawData;
                baseObj.PatternObjectList[i].Draw(sb, sqInfo, baseDrawData);
            }
        }

        /// <summary>
        /// Performs simple linear interpolation.
        ///
        /// �P���Ȑ��`��Ԃ����܂��B
        /// </summary>
        /// <param name="rate">
        /// If the interpolation rate is 1.0, the value will be that of "target".
        /// 
        /// ��Ԋ���1.0�Ȃ�target�̒l�ɂȂ�܂��B
        /// </param>
        /// <param name="fTarget">
        /// Target value
        /// 
        /// �ړI�l
        /// </param>
        /// <param name="fBase">
        /// Initial value
        /// 
        /// �����l
        /// </param>
        /// <returns>
        /// Interpolation results
        /// 
        /// ��Ԍ���
        /// </returns>
        private static float LinearInterporlation(float rate, float targetValue, 
            float baseValue)
        {
            return (targetValue * rate + baseValue * (1f - rate));
        }

        /// <summary>
        /// Performs linear interpolation for the conversion information 
        /// (position, rotation, scale, center point, and color).
        ///
        /// �ϊ����(�ʒu�E��]�E�X�P�[���E���S�_�E�F)����`��Ԃ��܂��B
        /// </summary>
        /// <param name="rate">
        /// Interpolation rate
        /// 
        /// ��Ԋ���
        /// </param>
        /// <param name="resultInfo">
        /// Calculation results will be stored.
        /// 
        /// �v�Z���ʂ�����܂��B
        /// </param>
        /// <param name="BaseInfo">
        /// Initial value
        /// 
        /// �����l
        /// </param>
        /// <param name="targetInfo">
        /// Target value
        /// 
        /// �ړI�l
        /// </param>
        private static void PutInfoLinearInterporlation(float rate, DrawData resultData,
                                DrawData baseData, DrawData targetData)
        {
            resultData.Position = new Point(
                (int)LinearInterporlation(rate, targetData.Position.X, 
                                            baseData.Position.X),
                (int)LinearInterporlation(rate, targetData.Position.Y, 
                                            baseData.Position.Y)
            );
            resultData.Color = new Color(
                (byte)LinearInterporlation(rate, targetData.Color.R, 
                                                        baseData.Color.R),
                (byte)LinearInterporlation(rate, targetData.Color.G, 
                                                        baseData.Color.G),
                (byte)LinearInterporlation(rate, targetData.Color.B, 
                                                        baseData.Color.B),
                (byte)LinearInterporlation(rate, targetData.Color.A, 
                                                        baseData.Color.A)
            );
            resultData.Scale = new Vector2(
                LinearInterporlation(rate, targetData.Scale.X, baseData.Scale.X),
                LinearInterporlation(rate, targetData.Scale.Y, baseData.Scale.Y));
            resultData.Center = new Point(
                (int)LinearInterporlation(rate, targetData.Center.X, 
                baseData.Center.X), 
                (int)LinearInterporlation(rate, targetData.Center.Y, 
                baseData.Center.Y));
            resultData.RotateZ = LinearInterporlation(rate, targetData.RotateZ, 
                baseData.RotateZ);
        }

        /// <summary>
        /// Performs spline interpolation by using TCB curve.
        ///
        /// TCB�Ȑ���p�����X�v���C����Ԃ��s���܂��B
        /// </summary>
        /// <param name="rate">
        /// Interpolation rate
        /// 
        /// ��Ԋ���
        /// </param>
        /// <param name="prevValue">
        /// Previous value of the initial value
        /// 
        /// �����l�̑O�̒l
        /// </param>
        /// <param name="baseValue">
        /// Initial value
        /// 
        /// �����l
        /// </param>
        /// <param name="targetValue">
        /// Target value
        /// 
        /// �ڕW�l
        /// </param>
        /// <param name="nextValue">
        /// Next value of the target value
        /// 
        /// �ڕW�l�̎��̒l
        /// </param>
        /// <returns>
        /// Calculation results
        /// 
        /// �v�Z����
        /// </returns>
        private float CalcSpline(float rate, float prevValue, 
            float baseValue, float targetValue, float nextValue)
        {
            float fRate = rate * rate,
                  fRate2 = fRate * rate;
            float fQ0 = tcbParam[0] * (baseValue - prevValue) + tcbParam[1] *
                        (targetValue - baseValue);
            float fQ1 = tcbParam[2] * (targetValue - baseValue) + tcbParam[3] * 
                        (nextValue - targetValue);

            return ((2.0f * baseValue - 2.0f * targetValue + fQ0 + fQ1) * fRate2 + 
                (-3.0f * baseValue + 3.0f * targetValue - 2.0f * fQ0 - fQ1) * 
                fRate + fQ0 * rate + baseValue);
        }

        /// <summary>
        /// Performs interpolation for the conversion information (position, rotation,
        /// scale, center position, color) by using TCB curve.
        ///
        /// TCB�Ȑ���p�����ϊ����(�ʒu�E��]�E�X�P�[���E���S�_�E�F)�̕�Ԃ��s���܂��B
        /// </summary>
        /// <param name="rate">
        /// Interpolation rate
        /// 
        /// ��Ԋ���
        /// </param>
        /// <param name="resultInfo">
        /// Calculation results
        /// 
        /// �v�Z����
        /// </param>
        /// <param name="prevInfo">
        /// Previous value of the initial value
        /// 
        /// �����l�̑O�̒l
        /// </param>
        /// <param name="baseInfo">
        /// Initial value
        /// 
        /// �����l
        /// </param>
        /// <param name="targetInfo">
        /// Target value
        /// 
        /// �ڕW�l
        /// </param>
        /// <param name="nextInfo">
        /// Next value of the target value
        /// 
        /// �ڕW�l�̎��̒l
        /// </param>
        private void PutInfoTCBSplineInterporlation(float rate,
            DrawData resultData, DrawData prevData, 
            DrawData baseData, DrawData targetData, DrawData nextData)
        {
            resultData.Position = new Point(
                (int)CalcSpline(rate, prevData.Position.X, baseData.Position.X, 
                                targetData.Position.X, nextData.Position.X),
                (int)CalcSpline(rate, prevData.Position.Y, baseData.Position.Y, 
                                targetData.Position.Y, nextData.Position.Y));
            resultData.Color = new Color(
                (byte)CalcSpline(rate, prevData.Color.R, baseData.Color.R, 
                                targetData.Color.R, nextData.Color.R),
                (byte)CalcSpline(rate, prevData.Color.G, baseData.Color.G, 
                                targetData.Color.G, nextData.Color.G),
                (byte)CalcSpline(rate, prevData.Color.B, baseData.Color.B, 
                                targetData.Color.B, nextData.Color.B),
                (byte)CalcSpline(rate, prevData.Color.A, baseData.Color.A, 
                                targetData.Color.A, nextData.Color.A));
            resultData.Scale = new Vector2(
                CalcSpline(rate, prevData.Scale.X, baseData.Scale.X, 
                                targetData.Scale.X, nextData.Scale.X),
                CalcSpline(rate, prevData.Scale.Y, baseData.Scale.Y, 
                                targetData.Scale.Y, nextData.Scale.Y));
            resultData.Center = new Point(
                (int)CalcSpline(rate, prevData.Center.X, baseData.Center.X, 
                                targetData.Center.X, nextData.Center.X),
                (int)CalcSpline(rate, prevData.Center.Y, baseData.Center.Y, 
                                targetData.Center.Y, nextData.Center.Y));
            resultData.RotateZ = CalcSpline(rate, prevData.RotateZ, 
                                baseData.RotateZ, targetData.RotateZ, nextData.RotateZ);
        }

        /// <summary>
        /// Resets the frame and plays it from the beginning.
        ///
        /// �t���[�������Z�b�g���čŏ�����Đ����܂��B
        /// </summary>
        public void Replay()
        {
            timeFrame = new TimeSpan();
        }

        /// <summary>
        /// Based on the conversion information obtained and interpolated by the 
        /// Update function, the sequence group displays pattern objects in the 
        /// pattern group that is referred to by the current Base sequence object.
        /// This function obtains this current Base sequence object.
        ///
        /// �V�[�P���X�O���[�v�́A���ڂ��Ă���V�[�P���X�I�u�W�F�N�g��
        /// �Q�Ƃ���p�^�[���O���[�v���̃p�^�[���I�u�W�F�N�g��
        /// Update�֐��ŋ��߂���Ԃ��ꂽ�ϊ����Ɋ�Â��ĕ\�����܂��B
        /// ���́A���ݒ��ڂ��Ă���V�[�P���X�I�u�W�F�N�g���擾����֐��ł��B
        /// </summary>
        /// <returns>
        /// Current sequence object to be displayed
        /// 
        /// ���ݕ\�����邱�ƂɂȂ��Ă���V�[�P���X�I�u�W�F�N�g
        /// </returns>
        [ContentSerializerIgnore]
        public SequenceObjectData CurrentObjectList
        {
            get
            {
                return (drawObjectId >= 0) ? objectList[drawObjectId] : objectList[0];
            }
        }

    }
}
