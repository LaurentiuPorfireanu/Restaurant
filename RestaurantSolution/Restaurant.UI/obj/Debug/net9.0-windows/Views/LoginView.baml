   M S B A M L   `   `   `  ����  Restaurant.UI#    Restaurant.UI.Views.LoginView   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Autentificare Restaurant��5   	   $	��420��5   	   $	��400��6    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�5
   	   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Autentificare Restaurant��6   $��0��5      $��24(�5      $
��Bold"�5      $��Center=�5      $��0,0,0,30q�5      �� 5!   
   $��1��6   $��0,0,0,15q�6"   .+�5"      �� $��Email:��6   $��14(�6&   $��0,0,0,5q�64   �� 5#      $��5q�5$      $��14(�5%      ��5#      �� *Email+ �� UpdateSourceTrigger$ PropertyChanged=�5%      5&      �� 5)   
   $��2��6   $��0,0,0,20q�6"   .+�5*      �� $��Parolă:��6   $��14(�6'   $��0,0,0,5q�65   G� 5+      -   5         G�Name$ PasswordBox��5+      $��5q�5,      $��14(�5-      5.      �� 51   
   $��3��6   
���  ��$
��Wrap=�54      $��0,0,0,15q�55      ��52      �� *ErrorMessage+��56      �� 57       �� 
TargetType" ��6    �� Triggers 58      �� 59       �� Binding 6&   �� *
HasError+	 �� Value		 False6C   .a�5:      �� 
 �� Property
 � y� �� Value$ 	Collapsed=�6;   5;      5=      5?      �� 5B   
   $��4��6   $��Autentificare��5C      $
��15,8q�5F      $��14(�5G      $��Center=�5H      $	��150��5I      $��0,0,0,15q�5J      ��5D      �� *LoginCommand+{�5E      �� *
CanLogin+5J      �� 5M   
   $��5��6   ���#DDDDDD$��0,1,0,0q�5O      $��0,5,0,15q�5P      ._�5Q      �� $	��sau��6   $��Center=�5R      $��Center=�5S      
�������$��	0,-10,0,0q�5U       �� Padding$
 10,0q�5V      5W      A� 5Z   
   $��6��6    A� Rows$ 1��6#    A� Columns$ 2��6,   $��0,0,0,15q�68   .+�5[      �� $��Continuă ca vizitator��6   $
��10,8q�5]      $��0,0,5,0q�5^      $��13(�5_      ��5\      �� *ContinueAsGuestCommand+5_      �� 5a      $��Creează cont��6   $
��10,8q�5c      $��5,0,0,0q�5d      $��13(�5e      ��5b      �� *CreateAccountCommand+5e      5f      � 5i   
   $��7��6    � IsIndeterminate$
 True��5j      $��5��5k      $	��Top=�5l      $��0,15,0,0q�5m      ��5n      �� 5o      " �6    5p      �� 5q       6&   �� *	IsLoading+		 False6D   .a�5r      �� 
 � y�$ 	Collapsed=�6;   5s      5u      5w      5x      5y      