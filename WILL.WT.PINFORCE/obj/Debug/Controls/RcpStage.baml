   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullE'clr-namespace:WILL.WT.PINFORCE.ControlsWILL.WT.PINFORCE.Controls  5      3clr-namespace:WILL.WT.PINFORCEWILL.WT.PINFORCE  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5      (    "WILL.WT.PINFORCE.Controls.RcpStage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:WILL.WT.PINFORCE.Controls     5      -emclr-namespace:WILL.WT.PINFORCE     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5      -   5          >�Name$  xRoot��5	      .��5
      (�  (� Stretch$
 Fill=�68    � DataContext 6   ��  �� ElementName$ xRoot��.��5   
   ��    xGroup3D#��[  6   ._�5      � $	��440��6   $	��773��6   $��10q�6,    68   �� *Recipe+ � ColumnDefinitions 5      �� 5      $	��440 �6'   �� 5      .+�5      ��  � 	Resources 5      (  "TS.FW.Wpf.v2.Controls.CaptionValue �� 
TargetType  xCaptionBase �� BasedOn �� Property	 �� Value��%[   '        7  �� 5      "  6    8   .�5      ��  � ��$	 50��67   5      5      .+�5        $��0��6&   
   Caption$
 	START O/D��63      SizeCaption$	 150Q�6G   .��5      $  TS.FW.Wpf.v2.Controls.ValueBtn    	xValueBtn#��[ 6&    �� ContentStringFormat$ {0} um��6c   
   Min$ 1��6�   
   Max$
 1000��6�   ��6I   �� *Start+6�   5        5      $��1��6&   $
 STEP O/D��63   $	 150Q�6F   .��5        #��[ 6&   $ {0} um��6b   $ 1��6�   $
 1000��6�   ��6I   �� *Step+6�   5        5       $��2��6&   $
 MAX. O/D��63   $	 150Q�6F   .��5!        #��[ 6&   $ {0} um��6a   $ 1��6�   $
 1000��6�   ��6I   �� *Max+6�   5"        5#      $��3��6&   $
 END O/D��63   $	 150Q�6E   .��5$        #��[ 6&   $ {0} um��6a   $ 1��6�   $
 1000��6�   ��6I   �� *End+6�   5%        5&      $��4��6&   $
 CONTACT TIME��63   $	 150Q�6J   .��5'        #��[ 6&   $ 
{0:f3} sec��6i   $	 0.1��6�   $ 10.000��6�   ��6I   �� *ContactTime+6�   5(        5)      $��5��6&   $
 
WORK SPEED��63   $	 150Q�6H   .��5*        #��[ 6&   $ {0:f3} mm/s��6g   $ 0.001��6�   $ 1.000��6�   ��6I   �� *	WorkSpeed+6�   5+        5,      $��6��6&   $
 STEP REPEAT��63   $	 150Q�6I   .��5-        #��[ 6&   $ {0} 회��6h   $ 1��6�   $	 100��6�   ��6I   �� *
StepRepeat+6�   5.        5/      $��7��6&   $
 REPEAT COUNT��63   $	 150Q�6J   .��50        #��[ 6&   $ {0} 회��6i   $ 1��6�   $	 100��6�   ��6I   �� *RepeatCount+6�   51        52      $��8��6&   $
 REPEAT METHOD��63   $	 150Q�6K   .��54      $  TS.FW.Wpf.v2.Controls.ComboBtn  $��2 2 2 2q�6&   ��67   �� !   WILL.WT.PINFORCE.EnumHelper   RepeatMethod �� Source# Z  �� Text 6|   �� *RepeatMethod+55        56      $��9��6&   $
 RELEASE USE��63   $	 150Q�6I   .��57      $  TS.FW.Wpf.v2.Controls.OnOffBtn     	IsChecked 6&   �� *
ReleaseUse+58        59      $��10��6&   $
 RELEASE TIME��64   $	 150Q�6K   .��5:        #��[ 6&   $ 
{0:f3} sec��6i   $	 0.1��6�   $ 10��6�   ��6I   �� *ReleaseTime+6�   5;        5<      $��11��6&   $
 RELEASE DISTANCE��64   $	 150Q�6O   .��5=        #��[ 6&   $ {0} um��6i   $ 1��6�   $
 1000��6�   ��6I   �� *ReleaseDist+6�   5>      5?      5@      5A      5B      5C      