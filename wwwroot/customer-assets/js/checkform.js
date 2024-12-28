const data = {
  vietnam: {
    "hanoi": ["Thanh Oai", "Ứng Hòa", "Hoàn Kiếm"],
    "hanam": ["Kim Bảng", "Phủ Lý"]
  },
  usa: {
    "california": ["Los Angeles", "San Diego", "San Francisco"],
    "newyork": ["Manhattan", "Brooklyn", "Queens"]
  }
};


// Cập nhật danh sách Tỉnh/Thành khi chọn Quốc gia
function updateProvinces() {
  const nationSelect = document.getElementById("nation").value;
  const citySelect = document.getElementById("city");
  const districtSelect = document.getElementById("district");

  // Reset các danh sách
  citySelect.innerHTML = '<option value="">Select City</option>';
  districtSelect.innerHTML = '<option value="">Select District</option>';

  // Kiểm tra và thêm các tỉnh/thành phù hợp
  if (nationSelect && data[nationSelect]) {
    Object.keys(data[nationSelect]).forEach(city => {
      const option = document.createElement("option");
      option.value = city;
      option.textContent = city.charAt(0).toUpperCase() + city.slice(1);
      citySelect.appendChild(option);
    });
  }
}

// Cập nhật danh sách Quận/Huyện khi chọn Tỉnh/Thành
function updateDistricts() {
  const nationSelect = document.getElementById("nation").value;
  const province = document.getElementById("city").value;
  const districtSelect = document.getElementById("district");

  // Reset danh sách Quận/Huyện
  districtSelect.innerHTML = '<option value="">Select District</option>';

  // Kiểm tra và thêm các quận/huyện phù hợp
  if (nationSelect && province && data[nationSelect][province]) {
    data[nationSelect][province].forEach(district => {
      const option = document.createElement("option");
      option.value = district;
      option.textContent = district;
      districtSelect.appendChild(option);
    });
  }
}

function check() {
  const name = document.getElementById('username').value.trim();
  const email = document.getElementById('email').value.trim();
  const phone = document.getElementById('phone').value.trim();
  const nation = document.getElementById('nation').value.trim();
  const city = document.getElementById('city').value.trim();
  const district = document.getElementById('district').value.trim();

  // Kiểm tra tất cả các giá trị hợp lệ
  if (
    name &&
    email &&
    phone &&
    nation !== 'Select Nation' &&
    city !== 'Select City' &&
    district !== 'Select District'
  ) {
    document.getElementById('insured_vehicle').classList.remove('hidden');
  }
}

const dataCar = {
  mec: {
    "a-class": ["A 180", "A 200", "A 250"],
    "c-class": ["C 180", "C 200", "C 250"]
  },
  toyota: {
    "camry": ["Camry 2.0", "Camry 2.5", "Camry 3.5"],
    "corolla": ["Corolla 1.8", "Corolla 2.0"]
  }
};

function carBrand() {
  const carBrandSelect = document.getElementById("car_brand").value;
  const vehicleLineSelect = document.getElementById("vehicle_line");
  const vehicleTypeSelect = document.getElementById("vehicle_type");

  // Reset các danh sách
  vehicleLineSelect.innerHTML = '<option value="">Vehicle Line</option>';
  vehicleTypeSelect.innerHTML = '<option value="">Vehicle Type</option>';

  // Kiểm tra và thêm các dòng xe phù hợp
  if (carBrandSelect && dataCar[carBrandSelect]) {
    Object.keys(dataCar[carBrandSelect]).forEach(vehicle_line => {
      const option = document.createElement("option");
      option.value = vehicle_line;
      option.textContent = vehicle_line.charAt(0).toUpperCase() + vehicle_line.slice(1);
      vehicleLineSelect.appendChild(option);
    });
  }
}

function vehicleLine() {
  const carBrandSelect = document.getElementById("car_brand").value;
  const vehicleLineSelect = document.getElementById("vehicle_line").value;
  const vehicleTypeSelect = document.getElementById("vehicle_type");

  // Reset danh sách loại xe
  vehicleTypeSelect.innerHTML = '<option value="">Vehicle Type</option>';

  // Kiểm tra và thêm các loại xe phù hợp
  if (carBrandSelect && vehicleLineSelect && dataCar[carBrandSelect][vehicleLineSelect]) {
    dataCar[carBrandSelect][vehicleLineSelect].forEach(vehicle_type => {
      const option = document.createElement("option");
      option.value = vehicle_type;
      option.textContent = vehicle_type;
      vehicleTypeSelect.appendChild(option);
    });
  }
}

function check2() {
  const carBrand = document.getElementById('car_brand').value.trim(); 
  const vehicleLine = document.getElementById('vehicle_line').value.trim();
  const vehicleType = document.getElementById('vehicle_type').value.trim();
  const year = document.getElementById('year_of_manufacture').value.trim();
  const seating = document.getElementById('seating').value.trim();
  const registration = document.getElementById('registrasion_date').value.trim();
  const numberPlate = document.getElementById('number_plate').value.trim();
  const frameNumber = document.getElementById('frame_number').value.trim(); 
  const machineNumber = document.getElementById('machine_number').value.trim();

  if(
    carBrand !== 'Car Brand' &&
    vehicleLine !== 'Vehicle Line' &&
    vehicleType !== 'Vehicle Type' &&
    year &&
    seating  &&
    registration  &&
    numberPlate &&
    frameNumber &&
    machineNumber
  ) {
    document.getElementById('driver_info').classList.remove('hidden');
  }
}

function checkabcxyz() {
  const birth = document.getElementById('date_of_birth').value.trim();
  const certificate = document.getElementById('date_of_certificate').value.trim();
  if(birth && certificate) {
    document.getElementById('contract_term').classList.remove('hidden');
  }
}

const dataBike = {
  honda: {
    "wave": ["Wave Alpha", "Wave RSX", "Wave Blade"],
    "winner": ["Winner 150", "Winner X"]
  },
  yamaha: {
    "exciter": ["Exciter 150", "Exciter 155"],
    "jupiter": ["Jupiter 150", "Jupiter 155"]
  }
}


function bikeBrand() {
  const bikeBrandSelect = document.getElementById("bike_brand").value;
  const bikeLineSelect = document.getElementById("bike_line");
  const bikeTypeSelect = document.getElementById("bike_type");

  // Reset các danh sách
  bikeLineSelect.innerHTML = '<option value="">bike Line</option>';
  bikeTypeSelect.innerHTML = '<option value="">bike Type</option>';

  // Kiểm tra và thêm các dòng xe phù hợp
  if (bikeBrandSelect && dataBike[bikeBrandSelect]) {
    Object.keys(dataBike[bikeBrandSelect]).forEach(bike_line => {
      const option = document.createElement("option");
      option.value = bike_line;
      option.textContent = bike_line.charAt(0).toUpperCase() + bike_line.slice(1);
      bikeLineSelect.appendChild(option);
    });
  }
}

function bikeLine() {
  const bikeBrandSelect = document.getElementById("bike_brand").value;
  const bikeLineSelect = document.getElementById("bike_line").value;
  const bikeTypeSelect = document.getElementById("bike_type");

  // Reset danh sách loại xe
  bikeTypeSelect.innerHTML = '<option value="">Vehicle Type</option>';

  // Kiểm tra và thêm các loại xe phù hợp
  if (bikeBrandSelect && bikeLineSelect && dataBike[bikeBrandSelect][bikeLineSelect]) {
    dataBike[bikeBrandSelect][bikeLineSelect].forEach(bike_type => {
      const option = document.createElement("option");
      option.value = bike_type;
      option.textContent = bike_type;
     bikeTypeSelect.appendChild(option);
    });
  }
}

function check2Bike() {
  const carBrand = document.getElementById('bike_brand').value.trim(); 
  const vehicleLine = document.getElementById('bike_line').value.trim();
  const vehicleType = document.getElementById('bike_type').value.trim();
  const year = document.getElementById('year_of_manufacture').value.trim();
  const registration = document.getElementById('registrasion_date').value.trim();
  const numberPlate = document.getElementById('number_plate').value.trim();
  const frameNumber = document.getElementById('frame_number').value.trim(); 
  const machineNumber = document.getElementById('machine_number').value.trim();

  if(
    carBrand !== 'Bike Brand' &&
    vehicleLine !== 'Bike Line' &&
    vehicleType !== 'Bike Type' &&
    year &&
  
    registration  &&
    numberPlate &&
    frameNumber &&
    machineNumber
  ) {
    document.getElementById('driver_info').classList.remove('hidden');
  }
}
