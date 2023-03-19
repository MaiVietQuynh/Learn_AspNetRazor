## Validation

# Validation
	- Muon biet tat ca du lieu khi submit co phu hop hay khong: ModelState.IsValid
	- Tao ra Validation rieng:
		+ Ke thua tu class ValidationAttribute
		+ override IsValid
# Binding
	- Tuy bien cach thuc Binding chuyen doi du lieu khi no gan vao Property, can thiep chuyen doi du lieu
	- Thuc hien tao ra ca Binder rieng:
		+ Tao ra cac class ke thua IModelBinder
		+ bindingContext: Chua du lieu truyen den
		+ Xem class UserNameBinding

## Upload File
	- FileExtensions: Chi lam viec tren du lieu string, ma du lieu hien tai la IFormFile nen se co loi du lieu khong phu hop
	- Tu xay dung class CheckFileExtensions