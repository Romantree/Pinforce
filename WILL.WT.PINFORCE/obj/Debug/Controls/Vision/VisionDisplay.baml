   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullS.clr-namespace:WILL.WT.PINFORCE.Controls.Vision WILL.WT.PINFORCE.Controls.Vision  5   	   A%clr-namespace:WILL.WT.PINFORCE.ModelsWILL.WT.PINFORCE.Models  5   	   G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5   	   4    .WILL.WT.PINFORCE.Controls.Vision.VisionDisplay   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   @local.clr-namespace:WILL.WT.PINFORCE.Controls.Vision     5   	   7model%clr-namespace:WILL.WT.PINFORCE.Models     5   	   Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5   	     � 	Resources  5         xVisionArea �� CornerRadius ��Name � RowDefinitions �� StringFormat��%q  &	        �� 5   
   .R�5      � .+�5      �� $	��2 5q�6   
���   �$ 5��69   ._�5      �� $ xImage��6   $��Uniform=�6,   $��0q�6a   ��6>   �� *SnapImageSource+y�6l   �� *VisionVisibility+5      �� 5      
���iii�$ 3��6.   $��12(�6?   $
��Bold"�6W   $
��Left=�6�   $	��Top=�6�   $��	15 20 0 0q�6�   ��6s   �� *
WindowSize+��6�   �� *
WindowSize+y�6�   �� *UIVisibility+._�5      � $��5q�6    5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5       �� $��2��6$   $
��DATA��68   
�������$��Center=�6W   ��6t   �� *
FontSize+�� 5"      $��1��6$   $��1��61   
�������$��Center=�6�   ��6A   �� *UI.AxisZ.State.ActPos+$ Z-Axis : {0:f3} mm����6�   �� *
FontSize+y�6�   �� �� 5#      $��2��6$   $��1��61   
�������$��Center=�6�   ��6A   �� *OD+$ Step O/D : {0:f1} um����6�   �� *
FontSize+y�6�   �� �� 5$      $��3��6$   $��1��61   
�������$��Center=�6�   ��6A   �� *UI.LoadcellDataModel_1.Data+$ LC #1 : {0:f3} g����6�   �� *
FontSize+y�6�   �� �� 5%      $��4��6$   $��1��61   
�������$��Center=�6�   ��6A   �� *UI.LoadcellDataModel_2.Data+$ LC #2 : {0:f3} g����6�   �� *
FontSize+y�6�   �� 5&      5'      5(      5)      5*      .��5-      &   TS.FW.Wpf.v2.Controls.GroupBox3D  $��VISION��6   .��5.   
   �  5/      �� 50      $��45 �6    �� 51      .+�54      �  � ColumnDefinitions 55      �� 56      $��2* �6'   �� 57      $��2* �6'   �� 58      �� 59      �� 5:      $��50 �6'   .+�5=      $  TS.FW.Wpf.v2.Controls.EventBtn  $��0��6     	xEventBtn#��[ 6.     appbar_control_play   Icon# [ 6q   $
��LIVE��6�   $
��LIVE��6�   
��� � ���6Q   �� *OnVisionCmd+6�     5>      $��1��6   #��[ 6.     appbar_control_stop# [ 6q   $
��STOP��6�   $
��STOP��6�   
���  ����6Q   �� *OnVisionCmd+6�     5?      $��2��6   #��[ 6.   $
��SAVE��6q   $
��SAVE��6�   ��6Q   �� *OnVisionCmd+6�     5@      $��3��6   #��[ 6.   $
��DATA��6q   ��6Q   �� *OnVisionCmd+6q     5A      $��4��6   #��[ 6.   $
��EXTD��6q      HorizontalIconAlignment$ Center=�6�   $��Center=�6�     appbar_magnify_add# [ 6�   
��������6Q   �� *OnVisionCmd+6�   5B      �� 5D      $��1��6   #��[  6*   ��6Y   �� 5E      5F      5G      