# Revise PageModel

## Razor
	- Lop phat sinh: obj/debug/....
	- Co the truy cap thuoc tinh o trong lop phat sinh bang cach:
		+ @abc;
		 @funtion{
			public string abc{set; get;}
		 }
	- Model trong Razor:
		+ Tao class ke thua PageModel: @model se truy cap duoc thuoc tinh cua lop do
	- Inject cac dich vu vao PageModel
	- Tao trang Razor bang lenh: dotnet new page -h
	- Tham so cua handler: tu dong duoc thiet lap dua tren gia tri truyen toi(tu cac form post du lieu, cac url, cac query,..)
##Model Binding
	- Co che lien ket du lieu
	- Du lieu gui den co dang: Key-Value
	- Nguon: 
		+ Form(html)
		+ Query(form html, get)
		+ Header
		+ Route Data: HttpRequest.RouteValues["Key"]
		+ Upload
		+ Body cua HttpRequest
	- De doc du lieu: HttpRequest(Controller, PageModel, View)
	-> It khi doc du lieu cach truc tiep nhu nay, ma ta dung co che Binding(du lieu duoc doc tu dong khi gui den)
	-Thuc hien Binding:
		+ Chi ro chi doc o nguon nao: [FromForm(Name="....")], [FromQuery(Name="...")],....
		+ Parameter Binding: 
			* O trong ca handler hay Action co cac tham so, khi thuc thi thi se tim tren tat ca nguon du lieu gui den de doc va chuyen thanh kieu du lieu tuong ung voi tham so va gan gia tri vao tham so			
		+ Property Binding:
			* [BindProperty]: Tim cac nguon du lieu gui den voi Key la theo ten cua property
			* Mac dinh chi Binnding voi phuong thuc post, neu muon phuong thuc Get thi [BindProperty(SupportsGet =true)]