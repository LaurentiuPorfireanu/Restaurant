   M S B A M L   `   `   `  ����  Restaurant.UI.    (Restaurant.UI.Views.Admin.AdminPanelView   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5	      � $��15q�6     � RowDefinitions  5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��0��6   $��0,0,0,15q�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� $��0��6   .+�5      �� $��Panou Administrare��6   $��24(�5      $��SemiBold"�5      $��0,0,0,5q�5      �� 5      $D��>Gestionează categorii, produse, meniuri, comenzi și rapoarte��6   $��14(�5       J� Opacity$	 0.7Q�5      $
��Wrap=�5      5      �� 5"      $��1��6   ���#FFF3E0���#FF9800$��1q�5%       �� CornerRadius$ 4��5&       �� Padding$ 10q�5'      ��5(      �� 5)       �� 
TargetType" ��6   .�5*      ��  �� Property � y� �� Value$ 	Collapsed=�67    �� Triggers 5+      �� 5,      	 �� Binding	 6*   �� *HasLowStockItems+
 �� Value
 True6O   .a�5-   "   ��  � y�$ Visible=�6?   5.      50      ._�52      ��  �� Orientation$ 
Horizontal=�6   .+�53      �� $	����6    $��Segoe MDL2 Assets)�54       ���#FF9800$��18(�56       $��Center=�57       $��0,0,8,0q�58       �� 59      .+�5:      �� $)��#Produse care se apropie de epuizare��6$   $��SemiBold"�5;   $   $
��Wrap=�5<   $   �� 5=      $	 0.8Q�5>   $   $��12(�5?   $   $
��Wrap=�5@   $   ��5=   $   �� *LowStockCount+ �� StringFormat$$ {0} produse necesită atenție��5@   $   5A      �� 5B      $��Vizualizează��6   $��10,0,0,0q�5D      ��5C      �� *ShowLowStockCommand+5D      5E      5F      5G      �� 5J   
   $��1��6    �� TabStripPlacement$	 Top=�6"    �� SelectedIndex 6:   �� *SelectedTabIndex+ � 	Resources 5K      ��%x   '��       �� 5L      " ��6   .�5M      ��  � ��$
 Auto��6/   �� 5N       � ��$ 30��62   5O      5P      .@�5S      �� $��	Categorii��6   .��5T      � $��10q�6     5U      �� 5V      $
��Auto �6(   �� 5W      $��* �6(   .+�5[      � $��0��6   $��0,0,0,15q�6(    5\      �� 5]      $��* �6/   �� 5^      $
��Auto �6/   �� 5_      $
��Auto �6/   .+�5b      �� $��0��6"   $��10q�5d      $��0,0,10,0q�5e      $��Center=�5f      ��5c      �� *CategoryName+ �� UpdateSourceTrigger$ PropertyChanged=�5f      �� 5h      $��1��6!   $��Adaugă��5i      $
��15,8q�5k      $��0,0,10,0q�5l      ��5j      �� *AddCategoryCommand+5l      �� 5n      $��2��6!   $��Actualizează��5o      $
��15,8q�5q      ��5p      �� *UpdateCategoryCommand+5q      5r      &   System.Windows.Controls.DataGrid  5u      $��1��6      AutoGenerateColumns$ False��5w         CanUserAddRows$ False��5x         SelectionMode$ Single=�5y      ��5v      �� *
Categories+ �� SelectedItem 5z      �� *SelectedCategory+   Columns 5{      0  *System.Windows.Controls.DataGridTextColumn  5|      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��61      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6\      
IsReadOnly$
 True��6g   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6=   �� *
CategoryId+6g     5}      $
 Nume��61   $ * 6X   $
 True��6b    6?   �� *Name+6b   4  .System.Windows.Controls.DataGridTemplateColumn  5~      $ Acțiuni��65   $
 Auto 6F      CellTemplate 5   "   �� 5�   &   .R�5�   *   �� $ 
Horizontal=�65   .+�5�   .   �� $��	Editează��65   $��0,0,5,0q�5�   )   $	��8,4q�5�   )   ��5�   )   �� *!DataContext.EditCategoryCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5�   )   �� 5�   )   �� 5�   .   $��Șterge��65   $	��8,4q�5�   )   ��5�   )   �� *#!DataContext.DeleteCategoryCommand+ ��  DataGrid��5�   )   �� 5�   )   5�   +   5�   '   5�      5�      5�      5�      �� 5�      $��	Preparate��6   .��5�      �� $
��Auto=�6   .��5�      � $��10q�6     5�      �� 5�      $
��Auto �6,   �� 5�      $��* �6,   .+�5�      � $��0��6   $��2��6,   y�6=   �� *IsAddPreparatMode+ '  #InverseBooleanToVisibilityConverter �� 	Converter# [    5�      �� 5�   "   $
��Auto �60   �� 5�   "   $��* �60   .+�5�      �� $��0��6%   $��Adaugă Preparat Nou��5�   %   $��Right=�5�   %   $
��15,8q�5�   %   $��0,0,0,15q�5�   %   ��5�   %   �� *AddNewPreparatCommand+5�   %     5�      $��1��6'   $ False��5�   '   $ False��5�   '   $ Single=�5�   '   ��5�   '   �� *	Preparate+ 5�   '   �� *SelectedPreparat+   RowStyle 5�   "   �� 5�   &   )  #System.Windows.Controls.DataGridRow"  6,   .�5�   *   ��  � ��$
 Auto��6C   �� 5�   *    � ��$ 25��6F   5�   '    5�   "     5�   &   $ ID��69   $ 50 6d   $
 True��6o    6E   �� *
PreparatID+6o     5�   &   $
 Nume��69   $ * 6`   $
 True��6j    6G   �� *Name+   ElementStyle 5�   *   �� 5�   .   " ��64   .�5�   2   ��  � ��$
 Wrap=�6Q   �� 5�   2    � ��$ Center=�6V   5�   /   5�   '     5�   &   $ Preț��69   $ 80 6~   $
 True��6�    6G   �� *Price+$ 
{0:N2} Lei��6�     5�   &   $ Cantitate/Porție��69   $	 120 6�   $
 True��6�    6S   �� *QuantityPortie+$
 {0}g��6�     5�   &   $ 
Stoc Total��69   $	 100 6�   $
 True��6�    6M   �� *QuantityTotal+$
 {0}g��6�     5�   &   $ 	Categorie��69   $	 120 6n   $
 True��6z    6L   �� *Category.Name+ 5�   *   �� 5�   .   " ��64   .�5�   2   ��  � ��$
 Wrap=�6Q   �� 5�   2    � ��$ Center=�6V   5�   /   5�   '     5�   &   $ Acțiuni��6=   $
 Auto 6N    5�   *   �� 5�   .   .R�5�   2   �� $ 
Horizontal=�6=   .+�5�   6   �� $��Șterge��6=   $	��8,4q�5�   =   ��5�   =   �� *#!DataContext.DeletePreparatCommand+ ��  DataGrid��5�   =   �� 5�   =   5�   3   5�   /   5�   '   5�      5�      � 5�      $��0��6   $��2��6,   y�5�      �� *IsAddPreparatMode+   BooleanToVisibilityConverter# [   5�      �� 5�   "   $
��Auto �60   �� 5�   "   $
��Auto �60   �� 5�   "   $��* �60   �� 5�   "   $
��Auto �60   .+�5�      �� $��0��6(   $��20(�5�   (   $��SemiBold"�5�   (   $��0,0,0,15q�5�   (   $
��Wrap=�5�   (   ��5�   (   �� *IsEditPreparatMode+  BooleanToStringConverter# [   �� ConverterParameter$-  'Editare Preparat|Adăugare Preparat Nou��5�   (   � 5�      $��1��6#   $��0,0,0,15q�60    5�   "   �� 5�   &   $��* �67   �� 5�   &   $��* �67   .+�5�   "   � $��0��6'   $��0,0,10,0q�67     5�   &   �� 5�   *   $
��Auto �68   �� 5�   *   $
��Auto �68   �� 5�   *   $
��Auto �68   �� 5�   *   $
��Auto �68   �� 5�   *   $
��Auto �68   .+�5   &   �� $��0��61   $��0,0,0,10q�6>   .+�5  *   �� $��	Denumire:��64   $��14(�6E   $��0,0,0,5q�6S   �� 5  *   ! �� TextWrapping$
! Wrap=�5  2   $��8q�5  2   $��14(�5  2   ��5  2   �� *PreparatName+$ PropertyChanged=�5  2   5  '   �� 5	  &   $��1��61   $��0,0,0,10q�6>   .+�5
  *   �� $��Preț (Lei):��64   $��14(�6G   $��0,0,0,5q�6U   �� 5  *   $��8q�5  2   $��14(�5  2   ��5  2   �� *PreparatPrice+$ PropertyChanged=�5  2   5  '   �� 5  &   $��2��61   $��0,0,0,10q�6>   .+�5  *   �� $��Cantitate porție (g):��64   $��14(�6Q   $��0,0,0,5q�6_   �� 5  *   $��8q�5  2   $��14(�5  2   ��5  2   �� *PreparatQuantityPortie+$ PropertyChanged=�5  2   5  '   �� 5  &   $��3��61   $��0,0,0,10q�6>   .+�5  *   �� $%��Cantitate totală în stoc (g):��64   $��14(�6Y   $��0,0,0,5q�6g   �� 5  *   $��8q�5  2   $��14(�5  2   ��5  2   �� *PreparatQuantityTotal+$ PropertyChanged=�5  2   5  '   �� 5!  &   $��4��61   $��0,0,0,10q�6>   .+�5"  *   �� $��
Categorie:��64   $��14(�6F   $��0,0,0,5q�6T   �� 5#  *   " �� DisplayMemberPath$
" Name��5%  3   $��8q�5&  3   $��14(�5'  3   ��5#  3   �� *
Categories+ 5$  3   �� *PreparatSelectedCategory+5'  3   5(  '   5)  #   � 5,  "   $��1��6'   $��10,0,0,0q�67     5-  &   �� 5.  *   $
��Auto �68   �� 5/  *   $��* �68   �� 50  *   $
��Auto �68   �� 51  *   $��* �68   .+�55  &   �� $��0��60   $��Alergeni disponibili:��56  /   $��14(�57  /   $��0,0,0,5q�58  /   �� 5;  &   $��1��6.   $
" Name��5=  -   $��0,0,0,10q�5>  -   $	��100��5?  -   ��5<  -   �� *PreparatAvailableAlergens+# � ContextMenu# 5@  *   �� 5A  .   .@�5B  2   m� $��Adaugă la preparat��6;   $ m� Command$ 5C  :   �� *AddPreparatAlergenCommand+% m� CommandParameter% 5D  :   �� *PlacementTarget.SelectedItem+ ��  ContextMenu5F  /   5H  '   �� 5K  &   $��2��60   $��Alergeni selectați:��5L  /   $��14(�5M  /   $��0,10,0,5q�5N  /   �� 5Q  &   $��3��6.   $
" Name��5S  -   $	��100��5T  -   ��5R  -   �� *PreparatSelectedAlergens+# 5U  *   �� 5V  .   .@�5W  2   m� $��Elimină din preparat��6;   $ 5X  ;   �� *RemovePreparatAlergenCommand+% 5Y  ;   �� *PlacementTarget.SelectedItem+ ��  ContextMenu5[  /   5]  '   5^  #   5_     � 5b     $��2��6#     5c  "   �� 5d  &   $
��Auto �64   �� 5e  &   $��* �64   .+�5i  "   � $��0��6'   $��0,0,0,10q�64    5j  &   �� 5k  *   $��* �6;   �� 5l  *   $
��Auto �6;   .+�5o  &   �� $��0��60   $��Imagini preparat:��5p  /   $��16(�5q  /   $��SemiBold"�5r  /   �� 5t  &   $��1��6-   $��Adaugă imagini��5u  -   $
��10,5q�5w  -   ��5v  -   �� *AddPreparatImagesCommand+5w  -   5x  #   �� 5{  "   $��1��6/   $
��Auto=�5|  -   $��Disabled=�5}  -   .��5~  &   �� ��63   �� *PreparatImages+��5  *   �� 5�  .   .R�5�  2   � $��0,0,0,10q�67    5�  6   �� 5�  :   $
��Auto �6K   �� 5�  :   $��* �6K   �� 5�  :   $
��Auto �6K   .+�5�  6   �� $��0��6=   $��80��5�  =   $��60��5�  =   $��1q�5�  =   ���#CCCCCC$��0,0,10,0q�5�  =   ._�5�  :   �� $��UniformToFill=�5�  ?   ��5�  @   �� *
FullPath+5�  ?   5�  7   �� 5�  6   $��1��6@   $��Center=�5�  ?   $
��Wrap=�5�  ?   ��5�  ?   �� *	ImagePath+5�  ?   �� 5�  6   $��2��6=   $��Șterge��5�  =   $	��8,3q�5�  =   ��5�  =   �� *(&DataContext.RemovePreparatImageCommand+ ��  ItemsControl��5�  =   �� 5�  =   5�  3   5�  /   5�  '   5�  #   5�     �� 5�     $��3��6)   $ 
Horizontal=�66   $��Right=�6O   $��0,15,0,0q�6k   .+�5�  "   �� $��	Anulează��6)   $
��15,8q�5�  )   $��0,0,10,0q�5�  )   ��5�  )   �� *CancelPreparatEditCommand+5�  )   �� 5�  "   $��	Salvează��6)   $
��15,8q�5�  )   ��5�  )   �� *SavePreparatCommand+{�5�  )   �� *CanSavePreparat+5�  )   5�     5�     5�     5�     5�     �� 5�     $��Meniuri��6   .��5�     �� $
��Auto=�6   .��5�     � .+�5�     � y�6   �� *MenuManagement.IsAddMode+# [    5�     �� 5�  "   $
��Auto �60   �� 5�  "   $��* �60   .+�5�     �� $��0��6%   $��Adaugă meniu nou��5�  %   $��Right=�5�  %   $��	0,10,0,10q�5�  %   $
��15,8q�5�  %   ��5�  %   �� *AddNewMenuCommand+5�  %     5�     $��1��6'   $ False��5�  %   $ False��5�  %   $ Single=�5�  %   ��5�  %   �� *Menus+ 5�  %   �� *SelectedMenu+ 5�  "   �� 5�  &   "  6,   .�5�  *   ��  � ��$
 Auto��6C   �� 5�  *    � ��$ 25��6F   5�  '    5�  "     5�  &   $ ID��69   $ 50 6`   $
 True��6k    6E   �� *MenuID+6k     5�  &   $
 Nume��69   $ * 6`   $
 True��6j    6G   �� *Name+ 5�  *   �� 5�  .   " ��64   .�5�  2   ��  � ��$
 Wrap=�6Q   �� 5�  2    � ��$ Center=�6V   5�  /   5�  '     5�  &   $ 	Categorie��69   $	 120 6n   $
 True��6z    6L   �� *Category.Name+ 5�  *   �� 5�  .   " ��64   .�5�  2   ��  � ��$
 Wrap=�6Q   �� 5�  2    � ��$ Center=�6V   5�  /   5�  '     5�  &   $ Nr. preparate��69   $	 100 6x   $
 True��6�    6P   �� *MenuPreparate.Count+6�     5�  &   $ Preț��69   $	 100 6�   $
 True��6�    6G   �� *CalculatedPrice+$ 
{0:N2} Lei��6�     5�  &   $ Acțiuni��6=   $
 Auto 6N    5�  *   �� 5�  .   .R�5�  2   �� $ 
Horizontal=�6=   .+�5�  6   �� $��Șterge��6=   $	��8,4q�5�  9   ��5�  9   �� *DataContext.DeleteMenuCommand+ ��  DataGrid��5�  9   �� 5�  9   5�  3   5�  /   5�  '   5�     5�     � 5�     y�6   �� *MenuManagement.IsAddMode+# [   5�     �� 5�  "   $
��Auto �60   �� 5   "   $
��Auto �60   �� 5  "   $��* �60   �� 5  "   $
��Auto �60   .+�5     �� $��0��6(   $��20(�5  %   $��SemiBold"�5	  %   $
��Wrap=�5
  %   $��	0,10,0,15q�5  %   ��5  %   �� *MenuManagement.IsEditMode+# [ $'  !Editare meniu|Adăugare meniu nou��5  %   � 5     $��1��6#   $��0,0,0,15q�60    5  "   �� 5  &   $��* �67   �� 5  &   $��* �67     5  "   �� 5  &   $
��Auto �64   �� 5  &   $
��Auto �64   .+�5  "   �� $��0��6-   $��0��6:   $��	0,0,10,10q�6J   .+�5  &   �� $��Nume meniu:��60   $��SemiBold"�6C   $��0,0,0,5q�6Y   �� 5  &   $
! Wrap=�5  -   $��5q�5  -   ��5  .   �� *MenuManagement.MenuName+$ PropertyChanged=�5  -   5  #   �� 5!  "   $��0��6-   $��1��6:   $��	10,0,0,10q�6J   .+�5"  &   �� $��
Categorie:��60   $��SemiBold"�6B   $��0,0,0,5q�6X   �� 5#  &   $
" Name��5%  -   $��5q�5&  -   ��5#  /   �� *
Categories+ 5$  -   �� *!MenuManagement.SelectedCategory+5&  -   5'  #   �� 5*  "   $��1��6-   $��0��6:   $��0,0,10,0q�6J   .+�5+  &   �� $��Reducere (%):��60   $��SemiBold"�6E   $��0,0,0,5q�6[   �� 5,  &   $��5q�5-  -   ��5,  .   �� *#!MenuManagement.DiscountPercentage+$ PropertyChanged=�5-  -   5.  #   � 51  "   $��1��6'   $��1��64   $��10,0,0,0q�6D     52  &   �� 53  *   $
��Auto �68   �� 54  *   $
��Auto �68   .+�57  &   �� $��0��60   $��Preț calculat:��6=   $��SemiBold"�6S   $��0,0,0,5q�6i   �� 58  &   $��1��61   $ 
Horizontal=�6>   .+�59  *   �� $��
Original: ��64   $��Center=�6F   �� 5:  *   $��Medium"�5;  1   $��Center=�5<  1   $��0,0,15,0q�5=  1   ��5:  4   �� *MenuManagement.TotalPrice+$ 
{0:N2} Lei��5=  1   �� 5>  *   $��Cu reducere: ��64   $��Center=�6I   �� 5?  *   $
��Bold"�5@  1   $��Center=�5A  1   
��� � ���5?  4   �� * MenuManagement.DiscountedPrice+$ 
{0:N2} Lei��5B  1   5C  '   5D  #   5E     � 5H     $��2��6#    5I  "   �� 5J  &   $��* �67   �� 5K  &   $
��Auto �67   �� 5L  &   $��* �67   .+�5P  "   � $��0��6'     5Q  &   �� 5R  *   $
��Auto �68   �� 5S  *   $��* �68   .+�5V  &   �� $��0��60   $��Preparate disponibile:��5W  -   $��SemiBold"�5X  -   $��0,0,0,10q�5Y  -     5[  &   $��1��6/   $ False��5]  -   $ False��5^  -   $ Single=�5_  -   ��5\  -   �� *#!MenuManagement.AvailablePreparate+ 5`  *   �� 5a  .   "  64   .�5b  2   ��  � ��$
 Auto��6K   �� 5c  2    � ��$ 25��6N   5d  /    5f  *     5g  .   $
 Nume��6A   $ * 6o   $
 True��6y    6O   �� *DisplayName+ 5h  2   �� 5i  6   " ��6<   .�5j  :   ��  � ��$
 Wrap=�6Y   �� 5k  :    � ��$ Center=�6^   5l  7   5n  /     5o  .   $ 	Categorie��6A   $	 100 6u   $
 True��6�    6T   �� *CategoryName+ 5p  2   �� 5q  6   " ��6<   .�5r  :   ��  � ��$
 Wrap=�6Y   �� 5s  :    � ��$ Center=�6^   5t  7   5v  /     5w  .   $ Preț��6A   $ 80 6r   $
 True��6}    6O   �� *PriceFormatted+6}     5x  .   $ Acțiuni��6E   $
 Auto 6V    5y  2   �� 5z  6   .R�5{  :   �� $��Adaugă��6A   $	��8,4q�5~  =   ��5|  =   �� */-DataContext.MenuManagement.AddPreparatCommand+ ��  DataGrid��5}  =   �� 5~  =   5  7   5�  /   5�  '   5�  #   �� 5�  "   $��1��6)   $��1��69   ���#CCCCCC$
��10,0q�6X   � 5�  "   $��2��6'     5�  &   �� 5�  *   $
��Auto �68   �� 5�  *   $��* �68   .+�5�  &   �� $��0��60   $��Preparate în meniu:��5�  -   $��SemiBold"�5�  -   $��0,0,0,10q�5�  -     5�  &   $��1��6/   $ False��5�  -   $ False��5�  -   $ Single=�5�  -   ��5�  -   �� *" MenuManagement.SelectedPreparate+ 5�  *   �� 5�  .   "  64   .�5�  2   ��  � ��$
 Auto��6K   �� 5�  2    � ��$ 25��6N   5�  /    5�  *     5�  .   $
 Nume��6A   $ * 6o   $
 True��6y    6O   �� *DisplayName+ 5�  2   �� 5�  6   " ��6<   .�5�  :   ��  � ��$
 Wrap=�6Y   �� 5�  :    � ��$ Center=�6^   5�  7   5�  /     5�  .   $ Standard��6A   $	 130 6x   $
 True��6�    6S   �� *StandardQuantity+6�     5�  .   $ Cantitate în meniu��6E   $	 150 6a    5�  2   �� 5�  6   .R�5�  :   �  5�  >   �� 5�  B   $��* �6S   �� 5�  B   $
��Auto �6S   .+�5�  >   �� $��0��6F   $��Center=�5�  E   ��5�  E   �� *QuantityMenuPortie+$ PropertyChanged=�5�  E   �� 5�  >   $��1��6H   $��g��5�  E   $��Center=�5�  E   $��5,0,0,0q�5�  E   5�  ;   5�  7   5�  /     5�  .   $ Preț��6A   $ 80 6r   $
 True��6}    6O   �� *PriceFormatted+6}     5�  .   $ Acțiuni��6E   $
 Auto 6V    5�  2   �� 5�  6   .R�5�  :   �� $��Elimină��6A   $	��8,4q�5�  =   ��5�  =   �� *20DataContext.MenuManagement.RemovePreparatCommand+ ��  DataGrid��5�  =   �� 5�  =   5�  7   5�  /   5�  '   5�  #   5�     �� 5�     $��3��6)   $ 
Horizontal=�5�  %   $��Right=�5�  %   $��0,15,0,0q�5�  %   .+�5�  "   �� $��	Anulează��6)   $
��15,8q�5�  %   $��0,0,10,0q�5�  %   ��5�  %   �� *MenuManagement.CancelCommand+5�  %   �� 5�  "   $��Salvează meniul��6)   $
��15,8q�5�  %   ��5�  %   �� * MenuManagement.SaveMenuCommand+{�5�  %   �� *MenuManagement.CanSaveMenu+5�     � 5�     $��0��6#   $��4��60   ���	#80FFFFFFy�5�  !   �� *MenuManagement.IsLoading+# [ .+�5�  "   �� $��Center=�6-   $��Center=�6H   .+�5�  &   �� $��Se încarcă...��60   $��Center=�6E   $��0,0,0,10q�6b   � 5�  &   & � IsIndeterminate$
& True��62   $	��200��6I   $��20��6U   5�  #   5�     5�     5�     5�     5�     �� 5�     $��Comenzi��6   .��5�     �� $
��Auto=�6   .��5�     � $��10q�6     5�     �� 5�     $
��Auto �6,   �� 5�     $��* �6,   .+�5�     �� $��0��6%   $ 
Horizontal=�5�  %   $��0,0,0,15q�5�  %   .+�5�     d� $��Toate comenzile��6+   $
��15,8q�5�  )   $��0,0,10,0q�5�  )   ' d� 	IsChecked' 5�  )   �� *IsAllOrdersSelected+( �� Mode$( TwoWay=�5�  )   d� 5�     $��Comenzi active��6+   $
��15,8q�5�  )   ' 5�  )   �� *IsActiveOrdersSelected+$( TwoWay=�5�  )   5�       5     $��1��6#   $ False��5  !   $ False��5  !   $ Single=�5  !   ��5  !   �� *Orders+ 5  !   �� *SelectedOrder+ 5     �� 5  "   "  6(   .�5	  &   ��  � ��$
 Auto��6?   �� 5
  &    � ��$ 25��6B   5  #    5       5  "   $ ID��65   $ 50 6]   $
 True��6h    6A   �� *	OrderID+6h     5  "   $	 Cod��65   $	 120 6`   $
 True��6l    6B   �� *	OrderCode+6l     5  "   $
 Data��65   $	 150 6j   $
 True��6v    6C   �� *OrderDateFormatted+6v     5  "   $ Client��65   $ * 6d   $
 True��6n    6E   �� *
ClientName+ 5  &   �� 5  *   " ��60   .�5  .   ��  � ��$
 Wrap=�6M   �� 5  .    � ��$ Center=�6R   5  +   5  #     5  "   $ Total��65   $ 90 6k   $
 True��6v    6D   �� *TotalCostFormatted+6v     5  "   $ Stare��69   $	 120 6H    5  &   �� 5  *   .R�5  .   �� $ 3��65   $	 5,2q�5  5   $
��Left=�5  5   ��5   2   �� 5!  6   ) �� Color) 6F   �� *StatusBackground+._�5#  2   �� 
�������$��12(�5%  9   $
��Wrap=�5&  9   ��5#  <   �� *
StatusText+5&  9   5'  /   5(  +   5*  #     5+  "   $ Acțiuni��69   $
 Auto 6J    5,  &   �� 5-  *   .R�5.  .   �� $ 
Horizontal=�69   .+�5/  2   �� $��Detalii��69   $��0,0,5,0q�52  9   $	��8,4q�53  9   ��50  9   �� *%#DataContext.ViewOrderDetailsCommand+ ��  DataGrid��51  9   �� 53  9   �� 54  2   $��Actualizează stare��69   $	��8,4q�57  9   ��55  9   �� *&$DataContext.UpdateOrderStatusCommand+ ��  DataGrid��56  9   �� ��58  6   �� 59  :   " ��6@   * �� BasedOn#* [B��6T   .�5:  >   ��  � y�$ 	Collapsed=�6[    5;  >   �� 5<  B   	 6N   �� *CanUpdateStatus+
 True6r   .a�5=  F   ��  � y�$ Visible=�6c   5>  C   5@  ;   5B  3   5C  /   5D  +   5F  #   5H     5I     5J     5K     �� 5N     $��Alergeni��6   .��5O     �� $
��Auto=�6   .��5P     � $��10q�6     5Q     �� 5R     $
��Auto �6,   �� 5S     $��* �6,   .+�5W     � $��0��6   $��0,0,0,15q�6,    5X     �� 5Y  "   $��* �63   �� 5Z  "   $
��Auto �63   �� 5[  "   $
��Auto �63   .+�5^     �� $��0��6&   $��10q�5`  %   $��0,0,10,0q�5a  %   $��Center=�5b  %   $
! Wrap=�5c  %   ��5_  %   �� *AlergenName+$ PropertyChanged=�5c  %   �� 5e     $��1��6%   $��Adaugă��5f  %   $
��15,8q�5h  %   $��0,0,10,0q�5i  %   ��5g  %   �� *AddAlergenCommand+��5j  "   �� 5k  &   " ��6,   #* [B��6@   .�5l  *   ��  � {�$ False��6F    5m  *   �� 5n  .   	 6:   �� *IsAddAlergenEnabled+
 True6b   .a�5o  2   ��  � {�$
 True��6N   5p  /   5r  '   5t     �� 5v     $��2��6%   $��Actualizează��5w  %   $
��15,8q�5y  %   ��5x  %   �� *UpdateAlergenCommand+��5z  "   �� 5{  &   " ��6,   #* [B��6@   .�5|  *   ��  � {�$ False��6F    5}  *   �� 5~  .   	 6:   �� *IsUpdateAlergenEnabled+
 True6e   .a�5  2   ��  � {�$
 True��6N   5�  /   5�  '   5�     5�       5�     $��1��6#   $ False��5�  !   $ False��5�  !   $ Single=�5�  !   ��5�  !   �� *
Alergens+ 5�  !   �� *SelectedAlergen+ 5�     �� 5�  "   "  6(   .�5�  &   ��  � ��$
 Auto��6?   �� 5�  &    � ��$ 25��6B   5�  #    5�       5�  "   $ ID��65   $ 50 6_   $
 True��6j    6A   �� *	AlergenID+6j     5�  "   $
 Nume��65   $ * 6\   $
 True��6f    6C   �� *Name+ 5�  &   �� 5�  *   " ��60   .�5�  .   ��  � ��$
 Wrap=�6M   �� 5�  .    � ��$ Center=�6R   5�  +   5�  #     5�  "   $ Acțiuni��69   $
 Auto 6J    5�  &   �� 5�  *   .R�5�  .   �� $ 
Horizontal=�69   .+�5�  2   �� $��	Editează��69   $��0,0,5,0q�5�  9   $	��8,4q�5�  9   ��5�  9   �� * DataContext.EditAlergenCommand+ ��  DataGrid��5�  9   �� 5�  9   �� 5�  2   $��Șterge��69   $	��8,4q�5�  9   ��5�  9   �� *" DataContext.DeleteAlergenCommand+ ��  DataGrid��5�  9   �� 5�  9   5�  /   5�  +   5�  #   5�     5�     5�     5�     �� 5�     $��Rapoarte��6   .��5�     � $��10q�6     5�     �� 5�     $
��Auto �6(   �� 5�     $
��Auto �6(   �� 5�     $��* �6(   .+�5�     �� $��0��6!   $��0,0,0,15q�6.   .+�5�     �� $!��Selectați tipul de raport:��6$   $��SemiBold"�5�     $��0,0,0,5q�5�     � 5�      5�     �� 5�  "   $	��300 �63   �� 5�  "   $
��Auto �63   �� 5�  "   $��* �63   .+�5�     �� $��0��6'   $
" Name��5�     $
��10,5q�5�     ��5�     �� *ReportTypes+ 5�     �� *SelectedReportType+5�     �� 5�     $��1��6%   $��Generează raport��5�     $��10,0,0,0q�5�     $
��10,5q�5�     ��5�     �� *GenerateReportCommand+5�     5�     5�     �� 5�     $��1��6!   $��0,0,0,15q�6.   ��5�     �� 5�     " ��6$   .�5�  "   ��  � y�$ 	Collapsed=�6?    5�  "   �� 5�  &   	 62   �� *ShowReportFilters+
 True6X   .a�5�  *   ��  � y�$ Visible=�6G   5�  '   5�     .+�5�     �� ���#E0E0E0$��1q�67   $ 10q�6K   $ 3��6X   ._�5�     �  5�  "   �� 5�  &   $
��Auto �67   �� 5�  &   $��* �67     5�  "   �� 5�  &   $
��Auto �64   �� 5�  &   $
��Auto �64   .+�5�  "   �� $��0��6,   $��0��69   $��Data început:��5�      $��0,0,10,0q�5�      $��Center=�5�      y�5�      �� *IsOrdersReportSelected+# [ (  "System.Windows.Controls.DatePicker  5�  "   $��0��6-   $��1��6:   $��0,0,0,5q�5�  !   +   SelectedDate+ 5�  !   �� *ReportStartDate+y�5�  !   �� *IsOrdersReportSelected+# [ �� 5�  "   $��1��6,   $��0��69   $��Data sfârșit:��5�      $��0,0,10,0q�5       $��Center=�5      y�5      �� *IsOrdersReportSelected+# [   5  "   $��1��6-   $��1��6:   + 5  !   �� *ReportEndDate+y�5  !   �� *IsOrdersReportSelected+# [ �� 5  "   $��0��6,   $��0��69   $��Prag stoc minim:��5	      $��0,0,10,0q�5
      $��Center=�5      y�5      �� *IsLowStockReportSelected+# [ �� 5  "   $��0��6*   $��1��67   $��0,0,0,5q�5     ��5     �� *LowStockThreshold+$ PropertyChanged=�y�5     �� *IsLowStockReportSelected+# [ �� 5  "   $��0��6,   $��0��69   $��Număr produse:��5      $��0,0,10,0q�5      $��Center=�5      y�5      �� *!IsPopularProductsReportSelected+# [ �� 5  "   $��0��6*   $��1��67   $��0,0,0,5q�5     ��5     �� *TopProductsCount+$ PropertyChanged=�y�5     �� *!IsPopularProductsReportSelected+# [ 5     5     5     � 5!     $��2��6   .+�5#     � y�6   �� *IsReportLoading+# [ .+�5$     �� ���	#80FFFFFF6(   �� 5%     $��Center=�6)   $��Center=�6D   .+�5&  "   � $
& True��6.   $	��100��6E   $��4��6Q   $��0,0,0,10q�6\   �� 5'  "   $��Se generează raportul...��6,   $��Center=�6L   5(     5)       5,     ,   
IsReadOnly$
, True��5-     $
 True��5.     ��5,  #   �� *
ReportData+��5/     �� 50  "   "  6(   .�51  &   ��  � y�$ 	Collapsed=�6C    52  &   �� 53  *   	 66   �� *HasReportData+
 True6X   .a�54  .   ��  � y�$ Visible=�6K   55  +   57  #   59     �� 5<     $K��ESelectați un tip de raport și apăsați butonul 'Generează raport'��6$   $��Center=�5=     $��Center=�5>     $	 0.7Q�5?     ��5@     �� 5A  "   " ��6(   .�5B  &   ��  � y�$ 	Collapsed=�6C    5C  &   �� 5D  *   	 66   �� *HasReportData+	
 False6X   .a�5E  .   ��  � y�$ Visible=�6K   5F  +   5H  #   5J     5K     5L     5M     5Q     5R     5T     