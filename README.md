Hoàn thiện game theo đề bài + kết hợp giải quyết cùng AI
Các chức năng hoàn thiện : + Tìm đường cho NPC
                           + Di chuyển NPC đến đích, tạo map ngẫu nhiên (kích thước 10x10 hoặc 20x20 tùy)
                           + Hiển thị thông báo khi NPC đi đến đích hoặc khi không tìm thấy đường đi
                           + Hiển thị UI phần biệt NPC, đích, tường, ô trống và đường đi khi tìm đường
                           + Có xử lý đa màn hình, UI không bị phần tai thỏ hay camera của điện thoại che
Cách chơi cụ thể: Chơi dựa trên các button hiển thị trên màn hình (GUI)
-Bao gồm 2 hộp điều khiển:
-Hộp 1: + Chọn "Find Path" để hiển thị đường đi tối ưu từ vị trí NPC đến đích (đường đi sẽ có màu vàng)
        + "Reset Path" xóa đường đi
        + "Generate Hardcoded Map": tạo map cố định đã setup từ đầu
        + "Generate New Map": tạo map mới ngầu nhiên ( nếu đang là map có kích thước 10x10 thì map mới ngẫu nhiên cũng là 10x10, tương tự với 20x20)
        + "Generate Map 10x10": Tạo map 10x10, sau khi bấm nút này sẽ tạo ngẫu nhiên map 10x10 (lúc này nếu bấm "Generate New Map" thì sẽ sinh map 10x10 ngẫu nhiên)
        + "Generate Map 20x20": Tạo map 20x20, sau khi bấm nút này sẽ tạo ngẫu nhiên map 20x20 (lúc này nếu bấm "Generate New Map" thì sẽ sinh map 20x20 ngẫu nhiên)
-Hộp 2: NPC Control: dùng để điều khiển NPC
       + "Start Move": nếu lựa chọn sau khi chọn "Find Path" thì NPC sẽ di chuyển theo path đã tìm được đến đích
       + "Stop Movement": khi lựa chọn sẽ dừng việc di chuyển của NPC lại
       + "Reset Position" : khi bấm sẽ reset vị trí của NPC về vị trí ban đầu
        
