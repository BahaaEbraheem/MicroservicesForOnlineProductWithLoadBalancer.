# خدمة الطلبات (Orders Service)

## نظرة عامة
خدمة إدارة الطلبات في نظام التجارة الإلكترونية الموزع. تستخدم قاعدة بيانات SQL Server لتخزين بيانات الطلبات وعناصر الطلبات.

## المتطلبات
- .NET 7.0 SDK
- SQL Server LocalDB (يتم تثبيته مع Visual Studio)
- Entity Framework Core Tools

## إعداد قاعدة البيانات
تم إعداد الخدمة لاستخدام SQL Server LocalDB مع الإعدادات التالية:

### Production Environment
```
Server=(localdb)\mssqllocaldb;Database=OrdersServiceDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

### Development Environment  
```
Server=(localdb)\mssqllocaldb;Database=OrdersServiceDB;Trusted_Connection=true;MultipleActiveResultSets=true
```

## تشغيل الخدمة

### الطريقة الأولى: استخدام ملف التشغيل
```bash
# تشغيل الملف المُعد مسبقاً
run-orders-service.bat
```

### الطريقة الثانية: الأوامر اليدوية
```bash
# استعادة الحزم
dotnet restore

# بناء المشروع
dotnet build

# تشغيل الخدمة
dotnet run --urls "http://localhost:5003"
```

## الوصول للخدمة
- **API Base URL**: `http://localhost:5003`
- **Swagger UI**: `http://localhost:5003`
- **Health Check**: `http://localhost:5003/health`

## ميزات قاعدة البيانات
- ✅ إنشاء قاعدة البيانات تلقائياً عند التشغيل الأول
- ✅ إضافة بيانات تجريبية (3 طلبات مع عناصرها)
- ✅ فهرسة الجداول لتحسين الأداء
- ✅ معالجة الأخطاء مع رسائل واضحة

## البيانات التجريبية
يتم إضافة البيانات التالية تلقائياً:
- 3 طلبات بحالات مختلفة (مُسلم، قيد المعالجة، في الانتظار)
- 5 عناصر طلب موزعة على الطلبات
- أسعار وكميات متنوعة

## استكشاف الأخطاء
إذا لم يتم إنشاء قاعدة البيانات:
1. تأكد من تثبيت SQL Server LocalDB
2. تحقق من صحة connection string
3. راجع رسائل الخطأ في وحدة التحكم
4. تأكد من وجود صلاحيات الكتابة في مجلد قاعدة البيانات
